using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.Todos.Seeders;

internal sealed class TodoItemSeeder(IApplicationDbContext dbContext, UserManager<UserEntity> userManager) : IDbSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // get the user id of the administrator
        var administratorUser = await userManager.FindByEmailAsync("administrator@localhost");
        if (administratorUser is null)
        {
            // if the user does not exist, we cannot seed the todo items
            return;
        }

        if (!dbContext.TodoItems.Any())
        {
            // create some sample todo items
            var todoItems = new List<TodoItemEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Todo Item 1",
                    Description = "This is a sample todo item.",
                    IsCompleted = false,
                    UserId = administratorUser.Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Todo Item 2",
                    Description = "This is another sample todo item.",
                    IsCompleted = false,
                    UserId = administratorUser.Id
                }
            };
            
            // add the todo items to the database
            await dbContext.TodoItems.AddRangeAsync(todoItems, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}