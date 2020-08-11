using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractFormMap: GSEntityTypeConfiguration<ContractForm>
    {
        public override void Configure(EntityTypeBuilder<ContractForm> builder)
        {
            builder.ToTable(nameof(ContractForm));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
