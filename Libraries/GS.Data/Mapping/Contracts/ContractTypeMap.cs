using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractTypeMap: GSEntityTypeConfiguration<ContractType>
    {
        public override void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.ToTable(nameof(ContractType));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
