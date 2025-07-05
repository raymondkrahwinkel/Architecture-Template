using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Domain;

public class Result : Result<object>
{
    public Result() : base(value: null)
    {
    }

    public Result(Error error) : base(error)
    {
    }
    
    public static Result Success() => new();
    
    public new static Result Failure(Error error) => new(error);
    
    public new static Result ValidationFailure(List<ValidationResult> validationResults)
    {
        var error = new Error("ValidationError", "One or more validation errors occurred.");
        return new Result(error) { ValidationResults = validationResults };
    }
}

public class Result<T>
{
    public T? Value { get; }
    public Error? Error { get; }
    
    public List<ValidationResult>? ValidationResults { get; set; } = [];
    
    public bool IsSuccess => Error is null && !ValidationResults.Any();

    public Result(T? value)
    {
        Value = value;
    }

    public Result(Error error)
    {
        Error = error;
    }

    public void AddToModelState(ModelStateDictionary modelState)
    {
        if(ValidationResults is not null && ValidationResults.Any())
        {
            foreach (var validationResult in ValidationResults)
            {
                if (!string.IsNullOrEmpty(validationResult.Message))
                {
                    modelState.AddModelError(validationResult.PropertyName, validationResult.Message);
                }
            }
        }
        else if (Error is not null)
        {
            modelState.AddModelError(string.Empty, Error.Message);
        }
    }
    
    public static Result<T> Success(T value) => new(value);
    
    public static Result<T> Failure(Error error) => new(error);
    
    public static Result<T> ValidationFailure(List<ValidationResult> validationResults)
    {
        var error = new Error("ValidationError", "One or more validation errors occurred.");
        return new Result<T>(error) { ValidationResults = validationResults };
    }
}