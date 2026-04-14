using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetFoodia.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        private readonly List<INotification> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents.Remove(eventItem);
        }
    }
}
