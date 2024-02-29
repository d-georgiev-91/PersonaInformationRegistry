using Microsoft.AspNetCore.Mvc;
using PersonalInformationRegistry.Application;
using System.Text.Json;

namespace PersonaInformationRegistry.Api.Middlewares;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ExceptionHandlingMiddleware
{
    private const string DefaultContentTypeForException = "application/json";
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = DefaultContentTypeForException;

        ProblemDetails? problemDetails;

        // There can be more than one exception, this could be extracted if it grows
        if (exception is NotFoundException)
        {
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "NotFound",
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
                Detail = exception.Message
            };
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing the request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = exception.Message
            };
        }

        context.Response.StatusCode = problemDetails.Status!.Value;
        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
