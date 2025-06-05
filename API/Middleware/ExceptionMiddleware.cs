using System;
using API.Errors;
using Azure.Core;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = env.IsDevelopment()
            ? new APIErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new APIErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");
            
        return context.Response.WriteAsJsonAsync(response);
    }
}
