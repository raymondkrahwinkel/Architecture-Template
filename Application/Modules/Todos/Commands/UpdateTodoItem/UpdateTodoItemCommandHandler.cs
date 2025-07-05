using Application.Common.Interfaces;
using Application.Common.Interfaces.Messaging;
using Application.Modules.Todos.Dtos;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Modules.Todos.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(TodoItemDto TodoItem) : ICommand;

internal sealed class UpdateTodoItemCommandHandler(IApplicationDbContext db, ILogger<UpdateTodoItemCommandHandler> logger) : ICommandHandler<UpdateTodoItemCommand>
{
    public async Task<Result> HandleAsync(UpdateTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (command.TodoItem.Id == null)
            {
                return Result.Failure(new Error("TodoItem.MissingId", "Todo item ID cannot be null."));
            }
            
            // get the existing todo item by id
            var existingTodoItem = await db.TodoItems.FirstOrDefaultAsync(t => t.Id == command.TodoItem.Id && t.UserId == command.TodoItem.UserId, cancellationToken);
            if (existingTodoItem == null)
            {
                return Result.Failure(new Error("TodoItem.NotFound", "Todo item not found."));
            }
            
            // update the existing todo item with the new values
            var todoItemEntity = command.TodoItem.ToEntity(existingTodoItem);
            todoItemEntity.AddDomainEvent(new ChangedTodoItemDomainEvent(todoItemEntity.Id));
            
            // update the entity in the database
            await db.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding or updating a todo item.");
            return Result.Failure(new Error("TodoItem.Exception", "An error occurred while processing your request. Please try again later."));
        }
    }
}