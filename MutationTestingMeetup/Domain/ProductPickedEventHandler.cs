using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductPickedEventHandler : IHandler<ProductPickedEvent>
    {
        public Task Handle(ProductPickedEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
