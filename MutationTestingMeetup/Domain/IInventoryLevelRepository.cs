using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IInventoryLevelRepository
    {
        Task<InventoryLevel> GetAsync(Guid id);

        InventoryLevel Create(InventoryLevel level);
    }
}
