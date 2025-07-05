using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    DbSet<TodoItemEntity> TodoItems { get; }
    
    Task MigrateAsync(CancellationToken cancellationToken = default);
    Task<bool> SeedAsync(CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}