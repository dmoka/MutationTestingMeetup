using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IStockLevelRepository
    {
        Task<StockLevel> GetAsync(Guid id);

        StockLevel Create(StockLevel level);
    }
}
