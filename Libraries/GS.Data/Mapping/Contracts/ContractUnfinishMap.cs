using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractUnfinishMap : GSEntityTypeConfiguration<ContractUnfinish>
    {
        public override void Configure(EntityTypeBuilder<ContractUnfinish> builder)
        {
            builder.ToTable(nameof(ContractUnfinish));
            builder.HasKey(t => t.Id);  
            base.Configure(builder);

            builder.HasOne(t => t.contract)
              .WithMany()
              .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.customer)
             .WithMany()
             .HasForeignKey(t => t.CreatorId);
            builder.HasOne(t => t.contractType)
             .WithMany()
             .HasForeignKey(t => t.ContractTypeId);
        }
    }
}
