using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public class StockLevel : BaseEntity
    {
        public Guid ProductId { get; }

        public int Count { get; private set; }

        public StockLevel(Guid productId, int count)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Count = count;
        }

        public void Decrease(int count)
        {
            Count -= count;
        }
    }
}
