using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductPickedEvent : IDomainEvent
    {
        public Product Product { get; }

        public ProductPickedEvent(Product product)
        {
            Product = product;
        }
    }
}
