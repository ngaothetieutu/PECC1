using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractSettlementSubMap : GSEntityTypeConfiguration<ContractSettlementSub>
    {
        public override void Configure(EntityTypeBuilder<ContractSettlementSub> builder)
        {
            builder.ToTable(nameof(ContractSettlementSub));
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();
            builder.HasOne(t => t.Contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.Tasks)
               .WithMany()
               .HasForeignKey(t => t.TaskId);
            builder.HasOne(t => t.ContractSettlement)
                .WithMany()
                .HasForeignKey(t => t.ContractSettlementId);

            base.Configure(builder);
        }
    }
}
