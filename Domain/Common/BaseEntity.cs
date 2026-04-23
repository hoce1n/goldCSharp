using Domain.Events;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }

        private readonly List<DomainEvent> _events = new();
        public IReadOnlyCollection<DomainEvent> Events => _events;

        protected void AddDomainEvent(DomainEvent eventItem)
        {
            _events.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            _events.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public bool HasEvents => _events.Count != 0;

        public void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}