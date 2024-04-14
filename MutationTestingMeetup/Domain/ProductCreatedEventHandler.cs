using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductCreatedEventHandler : IHandler<ProductCreatedEvent>
    {

        private readonly IStockLevelRepository _repository;

        public ProductCreatedEventHandler(IStockLevelRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(ProductCreatedEvent domainEvent)
        {
            var inventoryLevel = new StockLevel(domainEvent.ProductId, 10);

            _repository.Create(inventoryLevel);
            return Task.CompletedTask;
        }
    }
}
