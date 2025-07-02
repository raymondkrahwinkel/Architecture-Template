using Application.Common.Interfaces.Messaging;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Services.Implementations;

public class CqrsSender : ICqrsSender
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
        
        var type = typeof(ICommandHandler<>).MakeGenericType(request.GetType(), typeof(TResult));
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