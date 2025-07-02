using Domain;

namespace Application.Common.Interfaces.Messaging;

public interface IValidationHandler<in TRequest> where TRequest : IRequest
{
    Task<List<ValidationResult>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}