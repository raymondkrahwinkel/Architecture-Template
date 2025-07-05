using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class UserEntity : IdentityUser<Guid>, IEntity
{
    public ICollection<TodoItemEntity> TodoItems { get; set; } = new List<TodoItemEntity>();
    
    #region Domain Events
    private readonly List<IDomainEvent> _domainEvents = [];

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public List<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents;
    }
    #endregion
}