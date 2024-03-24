using System;

namespace MutationTestingMeetup.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; }
        public ProductCategory Category { get; }
        public decimal Price { get; }
        public bool IsOnSale { get; }

        public Product(string name, ProductCategory category, decimal price, bool isOnSale)
        {
            Id = Guid.NewGuid();
            Category = category;
            Price = price;
            IsOnSale = isOnSale;
            Name = name;
        }
    }
}
