namespace Domain;

public sealed record Error(string Code, string Message)
{
    public static Error NotFound(string message) => new("NotFound", message);
    public static Error InvalidInput(string message) => new("InvalidInput", message);
    public static Error Unauthorized(string message) => new("Unauthorized", message);
    public static Error Forbidden(string message) => new("Forbidden", message);
    public static Error InternalServerError(string message) => new("InternalServerError", message);
    
    public static implicit operator Result(Error error) => Result.Failure(error);
}