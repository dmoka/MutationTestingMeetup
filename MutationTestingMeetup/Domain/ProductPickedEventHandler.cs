using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class ProductPickedEventHandler : IHandler<ProductPickedEvent>
    {

        private readonly IStockLevelRepository _repository;

        public ProductPickedEventHandler(IStockLevelRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(ProductPickedEvent domainEvent)
        {
            var product = await _repository.GetAsync(domainEvent.ProductId);

            if (domainEvent.Count >= product.Count)
            {
                throw new ApplicationException("Cannot be picked more than stock level");
            }

            product.UpdateCount(domainEvent.Count);
        }

    }
}
