using Application.Common.Interfaces.Messaging;
using Application.Modules.Todos.Dtos;
using Domain;
using FluentValidation;

namespace Application.Modules.Todos.Commands.UpdateTodoItem;

public class UpdateTodoItemValidator(IValidator<TodoItemDto> validator) : IValidationHandler<UpdateTodoItemCommand>
{
    public Task<List<ValidationResult>> ValidateAsync(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request.TodoItem);
        if (validationResult.IsValid)
        {
            return Task.FromResult(new List<ValidationResult>());
        }
        
        var errors = validationResult.Errors.Select(e => new ValidationResult(e.PropertyName, e.ErrorMessage)).ToList();
        return Task.FromResult(errors);
    }
}