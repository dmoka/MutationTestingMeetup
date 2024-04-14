using System;

namespace MutationTestingMeetup.Domain
{
    public class Product : BaseEntity
    {
        public string Name { get; }
        public ProductCategory Category { get; }
        public decimal Price { get; }
        public bool IsOnSale { get; }

        public DateTime? LastPickedOn { get; private set;}

        public Product(string name, ProductCategory category, decimal price, bool isOnSale)
        {
            Id = Guid.NewGuid();
            Category = category;
            Price = price;
            IsOnSale = isOnSale;
            Name = name;

            RaiseDomainEvent(new ProductCreatedEvent(Id));

        }

        public void Pick(int count)
        {
            LastPickedOn = DateTime.Now;

            RaiseDomainEvent(new ProductPickedEvent(Id, count));
        }
    }
}
