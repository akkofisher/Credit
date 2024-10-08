using System.Reflection;
using System.Text;
using Credit.Application.Settings;
using Credit.Application.Settings.APIAuthorization;
using Credit.Application.Settings.RabbitMQ;
using Credit.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Credit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Serilog for logging with Seq
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(configuration["Seq:Url"]) 
            .CreateLogger();

        // Add Mediatr
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Add AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddControllers()
                  .ConfigureApiBehaviorOptions(options =>
                  {
                      options.InvalidModelStateResponseFactory = HandleModelException;
                  });

        //add fluent validation
        services.AddFluentValidationAutoValidation(configuration =>
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });

        //rabbitmq
        // Add RabbitMQ settings
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

        // Register RabbitMQ producer service
        services.AddSingleton<IRabbitMQService, RabbitMQService>();


        // add infrastructure services
        services.AddInfrastructure(configuration);

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddAuthentication(
                options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(
                jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = CreateTokenValidationParameters(configuration);
                });

        services
            .AddOptions<TokenSettings>()
            .Bind(configuration.GetSection("Token"))
            .ValidateOnStart();

        // api authorization
        services.AddScoped<IApiAuthorization, ApiAuthorization>();

        return services;
    }

    private static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
    {
        var result = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Token:Issuer"],
            ValidAudience = configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Token:Secret"]!)),
            RequireSignedTokens = false
        };

        return result;
    }

    //custom model validation error handling
    private static IActionResult HandleModelException(ActionContext context)
    {
        //add key and error message
        if (!context.ModelState.IsValid)
        {
            var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName);

           //add key and error message   
            var errorMessagesWithKey = string.Join(" | ", context.ModelState
                .SelectMany(kvp => kvp.Value.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}")));

            var request = context.HttpContext.Request;
            var logText = "Invalid Model " +
                          $"{Environment.NewLine}Error(s): {errorMessagesWithKey}" +
                          $"{Environment.NewLine}|{request.Method}| Full URL: {request.Path}{request.QueryString}";


            logger.LogError(logText);
        }

        return new BadRequestObjectResult(new
        {
            message = "Validation failed",
            errors = context.ModelState.SelectMany(kvp => kvp.Value.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}"))
    });
    }
}