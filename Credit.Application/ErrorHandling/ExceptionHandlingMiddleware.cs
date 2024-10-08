using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Credit.Application.ErrorHandling;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var logText = $"An error occurred: {exception.Message} {Environment.NewLine}  StackTrace: {exception.StackTrace} {Environment.NewLine} Inner Exception: {exception.InnerException} ";
        logger.LogError(logText);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var result = new
        {
            message = "A bad request occurred. Please check your request and try again.",
            error = exception.Message + $" - Inner Exception: {exception.InnerException}"
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}