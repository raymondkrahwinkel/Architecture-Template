using Domain.Abstractions;

namespace Application.Modules.Todos.Commands.UpdateTodoItem;

public record ChangedTodoItemDomainEvent(Guid Id) : IDomainEvent;

