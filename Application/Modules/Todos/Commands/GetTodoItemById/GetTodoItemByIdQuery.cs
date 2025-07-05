using Application.Common.Interfaces;
using Application.Common.Interfaces.Messaging;
using Application.Modules.Todos.Dtos;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Todos.Commands.GetTodoItemById;

public record GetTodoItemByIdQuery(Guid Id, Guid UserId) : IQuery<TodoItemDto>;

public sealed class GetTodoItemByIdQueryHandler(IApplicationDbContext db) : IQueryHandler<GetTodoItemByIdQuery, TodoItemDto>
{
    public async Task<Result<TodoItemDto>> HandleAsync(GetTodoItemByIdQuery query, CancellationToken cancellationToken)
    {
        var todoItem = await db.TodoItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id && x.UserId == query.UserId, cancellationToken);

        if (todoItem == null)
        {
            return Result<TodoItemDto>.Failure(new Error("TodoItem.NotFound", "The specified todo item was not found."));
        }

        return Result<TodoItemDto>.Success(TodoItemDto.FromEntity(todoItem));
    }
}