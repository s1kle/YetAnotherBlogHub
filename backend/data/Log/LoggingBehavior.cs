using MediatR;
using Serilog;

namespace BlogHub.Data.Log;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest 
    : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.Information("Request ({Name}): {@Request}", typeof(TRequest).Name, request);

        var response = await next();

        _logger.Information("Response ({Name}): {@Request}", typeof(TResponse).Name, response);

        return response;
    }
}