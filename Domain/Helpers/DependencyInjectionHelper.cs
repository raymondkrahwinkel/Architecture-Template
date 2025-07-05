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
            .AddClasses(classes => classes.AssignableTo(typeof(ITransientService)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            
            // scoped services
            .AddClasses(classes => classes.AssignableTo(typeof(IScopedService)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            
            // singleton services
            .AddClasses(classes => classes.AssignableTo(typeof(ISingletonService)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

        return services;
    }
    
    public static IServiceCollection BindDomainEventHandlers(
        this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        return services;
    }
}