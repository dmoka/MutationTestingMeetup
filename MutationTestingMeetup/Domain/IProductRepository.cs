using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid id);

        Task<List<Product>> GetAllAsync(ProductCategory category);
        Product Create(Product product);
    }
}
