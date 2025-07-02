using Application.Common.Interfaces.Messaging;
using Domain;

namespace Application.Modules.Todos.Commands.GetTodoItemsByUser;

public class GetTodoItemsByUserValidator : IValidationHandler<GetTodoItemsByUserQuery>
{
    public Task<List<ValidationResult>> ValidateAsync(GetTodoItemsByUserQuery query, CancellationToken cancellationToken)
    {
        var validationResults = new List<ValidationResult>();
        if (query.UserId == Guid.Empty)
        {
            validationResults.Add(new ValidationResult(nameof(query.UserId), "UserId is required."));
        }

        return Task.FromResult(validationResults);
    }
}