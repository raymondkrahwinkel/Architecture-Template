using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<UserEntity, UserRoleEntity, Guid>(options), IApplicationDbContext
{
    public DbSet<TodoItemEntity> TodoItems => Set<TodoItemEntity>();
}