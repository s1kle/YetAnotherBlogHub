using Serilog;

namespace BlogHub.Data.Common.Logging;

internal sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest
    : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Log.Information("Request ({Name}): {@Request}", typeof(TRequest).Name, request);

        var response = await next();

        Log.Information("Response ({Name}): {@Request}", typeof(TResponse).Name, response?.GetType());

        return response;
    }
}