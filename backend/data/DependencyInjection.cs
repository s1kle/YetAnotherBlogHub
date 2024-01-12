using System.Reflection;
using BlogHub.Data.Common.Logging;
using BlogHub.Data.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace BlogHub.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataDependencies(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}