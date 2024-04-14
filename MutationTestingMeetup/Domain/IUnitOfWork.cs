using System.Threading.Tasks;

namespace MutationTestingMeetup.Domain
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IStockLevelRepository Stocks { get; }

        Task CommitAsync();
    }
}
