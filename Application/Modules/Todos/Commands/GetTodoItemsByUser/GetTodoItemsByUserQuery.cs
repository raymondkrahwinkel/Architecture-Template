using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Todos.Commands.GetTodoItemsByUser;

public sealed record GetTodoItemsByUserQuery(Guid UserId) : IQuery<List<TodoItemEntity>>;

internal sealed class GetTodoItemsByUserQueryHandler : IQueryHandler<GetTodoItemsByUserQuery, List<TodoItemEntity>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetTodoItemsByUserQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<TodoItemEntity>>> HandleAsync(GetTodoItemsByUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TodoItems
            .Where(t => t.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        return new(result);
    }
}