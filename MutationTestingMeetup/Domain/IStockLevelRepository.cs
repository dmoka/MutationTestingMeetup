using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IStockLevelRepository
    {
        Task<StockLevel> GetAsync(Guid productId);

        StockLevel Create(StockLevel level);
    }
}
