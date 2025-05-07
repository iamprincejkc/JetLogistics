
using JetLogistics.Common.Common;
using Microsoft.Extensions.DependencyInjection;

namespace JetLogistics.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        return services;
    }
}
