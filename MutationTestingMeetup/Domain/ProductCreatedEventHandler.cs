using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductCreatedEventHandler : IHandler<ProductCreatedEvent>
    {
        private readonly IInventoryLevelRepository _repository;

        public ProductCreatedEventHandler(IInventoryLevelRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(ProductCreatedEvent domainEvent)
        {
            var inventoryLevel = new InventoryLevel(domainEvent.ProductId);

            _repository.Create(inventoryLevel);
            return Task.CompletedTask;
        }
    }
}
