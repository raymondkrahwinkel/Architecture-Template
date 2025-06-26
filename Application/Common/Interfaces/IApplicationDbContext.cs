using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    DbSet<TodoItemEntity> TodoItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}