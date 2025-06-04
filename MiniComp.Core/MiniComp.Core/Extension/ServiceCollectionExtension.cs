using Microsoft.Extensions.DependencyInjection;

namespace MiniComp.Core.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection Inject(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime,
        Type i,
        Type p
    )
    {
        var descriptor = new ServiceDescriptor(i, p, serviceLifetime);
        services.Add(descriptor);
        return services;
    }

    public static IServiceCollection Inject(
        this IServiceCollection services,
        string serviceKey,
        ServiceLifetime serviceLifetime,
        Type i,
        Type p
    )
    {
        var descriptor = new ServiceDescriptor(i, serviceKey, p, serviceLifetime);
        services.Add(descriptor);
        return services;
    }
}
