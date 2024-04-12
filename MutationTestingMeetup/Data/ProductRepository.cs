using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebShopDbContext _dbContext;
        private readonly DbSet<Product> _entities;

        public ProductRepository(WebShopDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
            _entities = dbContext.Set<Product>();
        }

        public Task<Product> GetAsync(Guid id)
        {
            return _entities.SingleOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<Product>> GetAllAsync(ProductCategory category)
        {
            return _entities.Where(p => p.Category == category).ToListAsync();
        }

        public Product Create(Product entity)
        {
            var entry = _entities.Add(entity);

            return entry.Entity;
        }

        public bool Exists(string name)
        {
            return _entities.Any(p => p.Name == name);
        }
    }
}
