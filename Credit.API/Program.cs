using Credit.Application;
using Credit.Application.ErrorHandling;
using Credit.Infrastructure.Seed;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Credit.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            //add swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Credit API", Version = "v1" });

                // Add JWT Bearer Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.SupportNonNullableReferenceTypes();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });

            // add application services
            builder.Services.AddApplication(builder.Configuration);

            //use serilog
            builder.Host.UseSerilog();

            // add authentication
            builder.Services.AddCustomAuthentication(builder.Configuration);

            var app = builder.Build();

            // Seed the database
              var appDbContext = builder.Services.BuildServiceProvider();
              DatabaseSeeder.Seed(appDbContext);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add the custom middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseCors(cors =>
                cors.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            app.Run();
            Log.CloseAndFlush();
        }
    }
}
