using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Services.Implementations;

internal sealed class DomainEventDispatcherService(IServiceProvider serviceProvider) : IDomainEventDispatcherService
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);
            
            foreach(var handler in handlers)
            {
                if (handler is null)
                {
                    continue; // skip if the handler does not implement the correct interface
                }
                
                // Use reflection to invoke the HandleAsync method
                var method = handlerType.GetMethod("HandleAsync");
                if (method == null)
                {
                    throw new InvalidOperationException($"Handler for {handlerType.Name} does not implement HandleAsync method.");
                }
        
                var task = method.Invoke(handler, [domainEvent, cancellationToken]);
                if (task is null)
                {
                    continue;
                }

                await (Task)task;
            }
        }
        
    }
}