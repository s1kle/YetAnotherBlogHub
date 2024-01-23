using System.Text.Json;
using BlogHub.Data.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BlogHub.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        Log.Error(exception, exception.Message);

        (var status, var title, var detail) = exception switch
        {
            NotFoundException e =>
                (StatusCodes.Status404NotFound,
                "Entity not found",
                e.Message),
            ValidationException =>
                (StatusCodes.Status400BadRequest,
                "Validation failed",
                $"The provided data failed one or more validation rules. Please check the values."),
            _ =>
                (StatusCodes.Status500InternalServerError,
                "Server error",
                "An internal server error has occurred."),
        };

        await WriteExceptionAsync(status, title, detail, context);
    }

    private async Task WriteExceptionAsync(int statusCode, string title, string detail, HttpContext context)
    {
        var problem = new ProblemDetails()
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        var json = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(json);
    }
}