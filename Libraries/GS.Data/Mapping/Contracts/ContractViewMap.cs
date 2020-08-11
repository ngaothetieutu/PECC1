using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractViewMap: GSEntityTypeConfiguration<ContractView>
    {
        public override void Configure(EntityTypeBuilder<ContractView> builder)
        {
            builder.ToTable(nameof(ContractView));
            builder.HasKey(t => t.Id);  
            base.Configure(builder);

            builder.HasOne(t => t.contract)
              .WithMany()
              .HasForeignKey(t => t.ContractId);
        }
    }
}
