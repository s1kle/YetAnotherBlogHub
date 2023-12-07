using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace BlogHub.Api.Filters;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger _logger;

    public LoggingFilter(ILogger logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.Information("Action Response: {@Response}", context.Result);
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.Information("Action Request: {@Request}", context.ActionDescriptor.DisplayName);
    }
}