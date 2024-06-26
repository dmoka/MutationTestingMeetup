﻿using System.Threading.Tasks;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WarehouseDbContext _dbContext;
        private IProductRepository _products;
        private IStockLevelRepository _stocks;

        public UnitOfWork(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IProductRepository Products => _products ??= new ProductRepository(_dbContext);
        public IStockLevelRepository Stocks => _stocks ??= new StockLevelRepository(_dbContext);



        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
