using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductPickedEvent : IDomainEvent
    {
        public Guid ProductId { get; }
        public int Count { get; }

        public ProductPickedEvent(Guid productId, int count)
        {
            ProductId = productId;
            Count = count;
        }
    }
}
