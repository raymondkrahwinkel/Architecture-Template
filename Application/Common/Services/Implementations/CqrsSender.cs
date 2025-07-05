using Application.Common.Interfaces.Messaging;
using Domain;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Services.Implementations;

internal sealed class CqrsSender : ICqrsSender
{
    private readonly IServiceProvider _serviceProvider;

    public CqrsSender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result<TResult>?> SendAsync<TResult>(IQuery<TResult> request, CancellationToken cancellationToken = default)
    {
        var validationResults = await _ExecuteValidationAsync(request, cancellationToken);
        if(validationResults.Count != 0)
        {
            return Result<TResult>.ValidationFailure(validationResults);
        }
        
        var type = typeof(IQueryHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(type);
        
        // Use reflection to invoke the HandleAsync method
        var method = type.GetMethod("HandleAsync");
        if (method == null)
        {
            throw new InvalidOperationException($"Handler for {type.Name} does not implement HandleAsync method.");
        }
        
        var task = method.Invoke(handler, [request, cancellationToken]);
        if (task is null)
        {
            return null;
        }

        await (Task)task;
        return (Result<TResult>?)method.ReturnType.GetProperty("Result")?.GetValue(task);
    }

    public async Task<Result?> SendAsync(ICommand request, CancellationToken cancellationToken = default)
    {
        var validationResults = await _ExecuteValidationAsync(request, cancellationToken);
        if(validationResults.Count != 0)
        {
            return Result.ValidationFailure(validationResults);
        }
        
        var type = typeof(ICommandHandler<>).MakeGenericType(request.GetType());
        var handler = _serviceProvider.GetRequiredService(type);
        
        // Use reflection to invoke the HandleAsync method
        var method = type.GetMethod("HandleAsync");
        if (method == null)
        {
            throw new InvalidOperationException($"Handler for {type.Name} does not implement HandleAsync method.");
        }
        
        var task = method.Invoke(handler, [request, cancellationToken]);
        if (task is null)
        {
            return null;
        }

        await (Task)task;
        return (Result?)method.ReturnType.GetProperty("Result")?.GetValue(task);
    }

    public async Task<Result<TResult>?> SendAsync<TResult>(ICommand<TResult> request, CancellationToken cancellationToken = default)
    {
        var validationResults = await _ExecuteValidationAsync(request, cancellationToken);
        if(validationResults.Count != 0)
        {
            return Result<TResult>.ValidationFailure(validationResults);
        }
        
        var type = typeof(ICommandHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(type);
        
        // Use reflection to invoke the HandleAsync method
        var method = type.GetMethod("HandleAsync");
        if (method == null)
        {
            throw new InvalidOperationException($"Handler for {type.Name} does not implement HandleAsync method.");
        }
        
        var task = method.Invoke(handler, [request, cancellationToken]);
        if (task is null)
        {
            return null;
        }

        await (Task)task;
        return (Result<TResult>?)method.ReturnType.GetProperty("Result")?.GetValue(task);
    }

    private async Task<List<ValidationResult>> _ExecuteValidationAsync(IRequest request, CancellationToken cancellationToken)
    {
        // check if we have a fluent validation available for the request
        var fluentValidationType = typeof(IValidator<>).MakeGenericType(request.GetType());
        var fluentValidator = _serviceProvider.GetService(fluentValidationType);
        if (fluentValidator != null)
        {
            // invoke the fluent validation
            var fluentValidateMethod = fluentValidationType.GetMethod("ValidateAsync");
            if (fluentValidateMethod == null)
            {
                throw new InvalidOperationException($"Validator for {fluentValidationType.Name} does not implement ValidateAsync method.");
            }

            var fluentValidatorTask = fluentValidateMethod.Invoke(fluentValidator, [request, cancellationToken]);
            if (fluentValidatorTask is null)
            {
                return [];
            }
            
            await (Task)fluentValidatorTask;
            var validationResult = (FluentValidation.Results.ValidationResult?)fluentValidateMethod.ReturnType.GetProperty("Result")?.GetValue(fluentValidatorTask);
            if (validationResult?.IsValid != true)
            {
                return validationResult?.Errors.Select(e => new ValidationResult(e.PropertyName, e.ErrorMessage))
                    .ToList() ?? [];
            }
        }
        
        var validatorType = typeof(IValidationHandler<>).MakeGenericType(request.GetType());
        var validatorHandler = _serviceProvider.GetService(validatorType);
        if (validatorHandler == null) return [];
        
        // invoke the validation handler
        var validateMethod = validatorType.GetMethod("ValidateAsync");
        if (validateMethod == null)
        {
            throw new InvalidOperationException($"Validator for {validatorType.Name} does not implement ValidateAsync method.");
        }

        var task = validateMethod.Invoke(validatorHandler, [request, cancellationToken]);
        if (task is null)
        {
            return [];
        }
        
        await (Task)task;
        return (List<ValidationResult>?)validateMethod.ReturnType.GetProperty("Result")?.GetValue(task) ?? [];
    }
}