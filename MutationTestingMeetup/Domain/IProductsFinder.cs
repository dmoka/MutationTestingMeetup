using System.Collections.Generic;
using System.Threading.Tasks;
using MutationTestingMeetup.Application.Controllers;

namespace MutationTestingMeetup.Domain
{
    public interface IProductsFinder
    {
        Task<IEnumerable<Product>> Find(ProductsQueryParameters queryParameters);
    }
}