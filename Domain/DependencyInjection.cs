using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // autoload the services from the domain layer
        services.AddServices(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}