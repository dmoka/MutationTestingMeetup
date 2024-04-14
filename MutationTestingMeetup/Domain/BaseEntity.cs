using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
