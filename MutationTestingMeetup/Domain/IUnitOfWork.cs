using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IInventoryLevelRepository Inventories { get; }

        Task CommitAsync();
    }
}
