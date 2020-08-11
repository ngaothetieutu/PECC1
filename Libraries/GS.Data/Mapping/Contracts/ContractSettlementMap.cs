using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractSettlementMap : GSEntityTypeConfiguration<ContractSettlement>
    {
        public override void Configure(EntityTypeBuilder<ContractSettlement> builder)
        {
            builder.ToTable(nameof(ContractSettlement));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Description).IsRequired();

            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.approver)
               .WithMany()
               .HasForeignKey(t => t.ApproverId);

            builder.Ignore(t => t.contractSettlementStatus);
            base.Configure(builder);
        }
    }
}
