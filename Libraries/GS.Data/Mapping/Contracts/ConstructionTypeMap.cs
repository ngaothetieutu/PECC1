using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ConstructionTypeMap: GSEntityTypeConfiguration<ConstructionType>
    {
        public override void Configure(EntityTypeBuilder<ConstructionType> builder)
        {
            builder.ToTable(nameof(ConstructionType));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired();

            builder.Ignore(t => t.contractCount);
            builder.Ignore(t => t.TypeId);

            base.Configure(builder);
        }
    }
}
