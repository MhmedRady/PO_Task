﻿using MediatR;
using Microsoft.Extensions.Logging;
namespace PO_Task.Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest

{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation(
                "Executing request {RequestName}",
                requestName);

            TResponse result = await next();

            _logger.LogInformation(
                "Request {RequestName} processed successfully",
                requestName);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Request {RequestName} processing failed",
                requestName);
            throw;
        }
    }
}
