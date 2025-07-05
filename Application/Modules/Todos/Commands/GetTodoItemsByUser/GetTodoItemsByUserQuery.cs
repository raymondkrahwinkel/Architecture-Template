using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces;
using Application.Modules.Todos.Dtos;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Todos.Commands.GetTodoItemsByUser;

public sealed record GetTodoItemsByUserQuery(Guid UserId) : IQuery<List<TodoItemDto>>;

internal sealed class GetTodoItemsByUserQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTodoItemsByUserQuery, List<TodoItemDto>>
{
    public async Task<Result<List<TodoItemDto>>> HandleAsync(GetTodoItemsByUserQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext.TodoItems
            .Where(t => t.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        return new Result<List<TodoItemDto>>(result.Select(TodoItemDto.FromEntity).ToList());
    }
}