using System.Reflection;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Attributes;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcherService domainEventDispatcherService, IServiceProvider serviceProvider, ILogger<ApplicationDbContext> logger) 
    : IdentityDbContext<UserEntity, UserRoleEntity, Guid>(options), IApplicationDbContext
{
    public DbSet<TodoItemEntity> TodoItems => Set<TodoItemEntity>();
    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var seeders = serviceProvider.GetServices<IDbSeeder>();
            seeders = seeders.OrderBy(s => (s.GetType().GetCustomAttribute(typeof(OrderAttribute)) as OrderAttribute)?.Order ?? int.MaxValue).ToList();

            // execute each seeder
            foreach (var seeder in seeders)
            {
                try
                {
                    await seeder.SeedAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while seeding database with {SeederName}", seeder.GetType().Name);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding database");
        }

        return true;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // store the changed entities that implement IEntity
        var changedEntities = ChangeTracker
            .Entries<IEntity>()
            .Select(x => x.Entity)
            .ToList();
        
        int result = await base.SaveChangesAsync(cancellationToken);
        
        // call the domain event handlers after the changes are saved
        await PublishDomainEventsAsync(changedEntities, cancellationToken);
        
        return result;
    }
    
    private async Task PublishDomainEventsAsync(List<IEntity> changedEntities, CancellationToken cancellationToken)
    {
        // get the domain events from the changed entities again
        var domainEvents = changedEntities
            .SelectMany(entity => entity.GetDomainEvents())
            .ToList();
        
        await domainEventDispatcherService.DispatchAsync(domainEvents, cancellationToken);
        
        // remove the domain events from the changed entities
        foreach (var entity in changedEntities)
        {
            entity.ClearDomainEvents();
        }
    }
}