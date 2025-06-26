using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        
        // todo: does this belong in the infrastructure layer?
        services.AddDefaultIdentity<UserEntity>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<UserRoleEntity>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();
        
        return services;
    }
}