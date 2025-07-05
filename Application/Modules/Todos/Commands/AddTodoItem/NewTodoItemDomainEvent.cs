using Domain.Abstractions;

namespace Application.Modules.Todos.Commands.AddTodoItem;

public record NewTodoItemDomainEvent(Guid Id) : IDomainEvent;

