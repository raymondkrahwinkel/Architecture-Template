using Application.Modules.Todos.Commands.AddTodoItem;
using Application.Modules.Todos.Commands.UpdateTodoItem;
using Domain.Abstractions;

namespace Application.Modules.Todos;

public static class TodoItemEvents
{
    public static event EventHandler<Guid>? TodoItemAdded;
    internal static void RaiseTodoItemAdded(Guid id)
    {
        TodoItemAdded?.Invoke(null, id);
    }
    
    public static event EventHandler<Guid>? TodoItemChanged;
    internal static void RaiseTodoItemChanged(Guid id)
    {
        TodoItemChanged?.Invoke(null, id);
    }
}

internal class NewTodoItemDomainEventHandler : IDomainEventHandler<NewTodoItemDomainEvent>
{
    public Task HandleAsync(NewTodoItemDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        TodoItemEvents.RaiseTodoItemAdded(domainEvent.Id);
        return Task.CompletedTask;
    }
}

internal class ChangedTodoItemDomainEventHandler : IDomainEventHandler<ChangedTodoItemDomainEvent>
{
    public Task HandleAsync(ChangedTodoItemDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        TodoItemEvents.RaiseTodoItemChanged(domainEvent.Id);
        return Task.CompletedTask;
    }
}