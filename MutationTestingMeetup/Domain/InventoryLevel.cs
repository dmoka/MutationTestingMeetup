using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class InventoryLevel : BaseEntity
    {
        public Guid ProductId { get; }

        public int Count { get; }

        public InventoryLevel(Guid productId)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Count = 0;
        }

    }
}
