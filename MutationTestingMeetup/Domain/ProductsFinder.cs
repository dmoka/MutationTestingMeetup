using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MutationTestingMeetup.Application.Controllers;

namespace MutationTestingMeetup.Domain
{
    public class ProductsFinder : IProductsFinder
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsFinder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> Find(ProductsQueryParameters queryParameters)
        {
            var products = await GetAllProductsByCategory(queryParameters.Category);

            products = FilterByMaxPriceIfSpecified(products, queryParameters.MaxPrice);
            products = FilterByIsOnSaleIfSpecified(products, queryParameters.IsOnSale);

            return products.OrderBy(p => p.Name);
        }

        private async Task<List<Product>> GetAllProductsByCategory(ProductCategory? category)
        {
            return await _unitOfWork.Products.GetAllAsync(category.Value);
        }

        private static List<Product> FilterByMaxPriceIfSpecified(List<Product> products, decimal? maxPrice)
        {
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice).ToList();
            }

            return products;
        }

        private static List<Product> FilterByIsOnSaleIfSpecified(List<Product> products, bool? isOnSale)
        {
            if (isOnSale.HasValue)
            {
                products = products.Where(p => p.SaleState == SaleState.OnSale).ToList();
            }

            return products;
        }
    }
}
