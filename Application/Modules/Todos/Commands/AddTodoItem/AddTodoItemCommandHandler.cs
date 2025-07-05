using Application.Common.Interfaces;
using Application.Common.Interfaces.Messaging;
using Application.Modules.Todos.Dtos;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Modules.Todos.Commands.AddTodoItem;

public record AddTodoItemCommand(TodoItemDto TodoItem) : ICommand<Guid>;

internal sealed class AddTodoItemCommandHandler(IApplicationDbContext db, ILogger<AddTodoItemCommandHandler> logger) : ICommandHandler<AddTodoItemCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddTodoItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var todoItemEntity = command.TodoItem.ToEntity();
            todoItemEntity.AddDomainEvent(new NewTodoItemDomainEvent(todoItemEntity.Id));
            await db.TodoItems.AddAsync(todoItemEntity, cancellationToken: cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            
            return Result<Guid>.Success(todoItemEntity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while adding or updating a todo item.");
            return Result<Guid>.Failure(new Error("TodoItem.Exception", "An error occurred while processing your request. Please try again later."));
        }
    }
}