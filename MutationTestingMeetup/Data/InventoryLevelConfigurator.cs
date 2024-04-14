using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class InventoryLevelConfigurator : EntityConfigurationBase<StockLevel>
    {


        public override void Configure(EntityTypeBuilder<StockLevel> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(p => p.ProductId)
                .IsRequired();

            builder.Property(p => p.Count)
                .IsRequired();
        }
    }
}
