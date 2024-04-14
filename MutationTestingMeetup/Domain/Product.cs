using System;

namespace MutationTestingMeetup.Domain
{
    public class Product : BaseEntity
    {
        public string Name { get; }
        public ProductCategory Category { get; }
        public decimal Price { get; }
        public bool IsOnSale { get; }
        public PickState LastPickState { get; private set; }

        public Product(string name, ProductCategory category, decimal price, bool isOnSale)
        {
            Id = Guid.NewGuid();
            Category = category;
            Price = price;
            IsOnSale = isOnSale;
            Name = name;
            LastPickState = PickState.New;

            RaiseDomainEvent(new ProductCreatedEvent(Id));

        }

        public void Pick(int count)
        {
            LastPickState = PickState.Picked;

            RaiseDomainEvent(new ProductPickedEvent(Id, count));
        }
    }

    public enum PickState
    {
        New,
        Picked,
    }
}
