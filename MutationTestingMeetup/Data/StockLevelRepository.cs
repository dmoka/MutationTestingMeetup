using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class StockLevelRepository : IStockLevelRepository
    {
        private readonly WebShopDbContext _dbContext;
        private readonly DbSet<StockLevel> _entities;

        public StockLevelRepository(WebShopDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
            _entities = dbContext.Set<StockLevel>();
        }

        public Task<StockLevel> GetAsync(Guid id)
        {
            return _entities.SingleOrDefaultAsync(p => p.Id == id);
        }


        public StockLevel Create(StockLevel entity)
        {
            var entry = _entities.Add(entity);

            return entry.Entity;
        }

    }
}
