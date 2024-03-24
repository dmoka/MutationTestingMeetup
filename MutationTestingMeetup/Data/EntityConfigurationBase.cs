using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MutationTestingMeetup.Data
{
    public abstract class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        protected const string PriceColumnType = "decimal(12,2)";

        public string ColumnTypeNVarChar(int length) => $"nvarchar({length})";

        public abstract void Configure(EntityTypeBuilder<TEntity> builder);

    }
}
