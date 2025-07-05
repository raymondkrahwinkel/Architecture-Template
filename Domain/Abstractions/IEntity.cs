namespace Domain.Abstractions;

public interface IEntity
{
    void AddDomainEvent(IDomainEvent domainEvent);
    void RemoveDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
    List<IDomainEvent> GetDomainEvents();
}