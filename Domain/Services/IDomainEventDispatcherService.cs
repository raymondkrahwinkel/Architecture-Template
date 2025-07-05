using Domain.Abstractions;

namespace Domain.Services;

public interface IDomainEventDispatcherService : IScopedService
{
    /// <summary>
    /// Dispatches a collection of domain events asynchronously
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvent, CancellationToken cancellationToken = default);
}