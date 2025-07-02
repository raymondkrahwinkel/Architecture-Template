using System.Reflection;
using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Helpers;

public static class DependencyInjectionHelper
{
    public static IServiceCollection AddServices(this IServiceCollection services, Assembly assembly)
    {
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            // transient services
            .AddClasses(classes => classes.AssignableTo(typeof(ITransientService)))
            .AsSelfWithInterfaces()
            .WithTransientLifetime()
            
            // scoped services
            .AddClasses(classes => classes.AssignableTo(typeof(IScopedService)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime()
            
            // singleton services
            .AddClasses(classes => classes.AssignableTo(typeof(ISingletonService)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime()
        );

        return services;
    }
}