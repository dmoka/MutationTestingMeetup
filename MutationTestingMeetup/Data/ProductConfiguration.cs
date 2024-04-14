using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Data
{
    public class ProductEntityConfiguration : EntityConfigurationBase<Product>
    {

        private readonly EnumToStringConverter<ProductCategory> productCategoryToStringConverter = new EnumToStringConverter<ProductCategory>();

        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name)
                .HasColumnType(ColumnTypeNVarChar(100));

            builder
                .Property(e => e.Category)
                .HasColumnType(ColumnTypeNVarChar(20))
                .HasConversion(productCategoryToStringConverter)
                .HasDefaultValue(ProductCategory.NotSpecified);

            builder.Property(p => p.Price)
                .HasColumnType(PriceColumnType)
                .IsRequired();

            builder.Property(p => p.SaleState)
                .IsRequired();
        }
    }
}
