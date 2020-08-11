using GS.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Customers
{
    public class UnitMap : GSEntityTypeConfiguration<Unit>
    {
        public override void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable(nameof(Unit));
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.parentUnit)
              .WithMany()
              .HasForeignKey(t => t.ParentId);
            builder.HasOne(t => t.stores)
              .WithMany()
              .HasForeignKey(t => t.StoreId);
            base.Configure(builder);
        }
    }
}
