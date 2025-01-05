using Microsoft.AspNetCore.Mvc;
using PO_Task.Application.Exceptions;
using PO_Task.Domain.BuildingBlocks;

namespace PO_Task.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Exception occurred: {Message}",
                exception.Message);

            ExceptionDetails exceptionDetails = GetExceptionDetails(exception);

            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => 
                new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "Validation error",
                    "One or more validation errors has occurred",
                validationException.Errors),
            ApplicationFlowException applicationFlowException =>
                new ExceptionDetails(
                    StatusCodes.Status406NotAcceptable,
                    "ApplicationFlowError",
                    "Application flow error",
                    "An error has occurred during the application flow",
                applicationFlowException.Errors),
            BusinessRuleException businessRuleValidationException => 
                new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "BusinessRuleValidationFailure",
                    "Business rule validation error",
                    "a business rule validation errors has occurred",
                [businessRuleValidationException.Errors]),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "An unexpected error has occurred",
                exception.Message,
                null)
        };
    }

    private sealed record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);
}
