using System.Threading.Tasks;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebShopDbContext _dbContext;
        private IProductRepository _products;

        public UnitOfWork(WebShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IProductRepository Products => _products ??= new ProductRepository(_dbContext);


        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
