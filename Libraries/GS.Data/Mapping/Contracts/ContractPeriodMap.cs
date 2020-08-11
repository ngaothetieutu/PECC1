using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractPeriodMap: GSEntityTypeConfiguration<ContractPeriod>
    {
        public override void Configure(EntityTypeBuilder<ContractPeriod> builder)
        {
            builder.ToTable(nameof(ContractPeriod));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
