using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class InventoryLevelRepository : IInventoryLevelRepository
    {
        private readonly WebShopDbContext _dbContext;
        private readonly DbSet<InventoryLevel> _entities;

        public InventoryLevelRepository(WebShopDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
            _entities = dbContext.Set<InventoryLevel>();
        }

        public Task<InventoryLevel> GetAsync(Guid id)
        {
            return _entities.SingleOrDefaultAsync(p => p.Id == id);
        }


        public InventoryLevel Create(InventoryLevel entity)
        {
            var entry = _entities.Add(entity);

            return entry.Entity;
        }

    }
}
