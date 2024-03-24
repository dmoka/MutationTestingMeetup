using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Application.Controllers
{
    public class CreateProductPayload
    {
        public string Name  { get; set; }

        public ProductCategory Category { get; set; }

        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }
    }
}
