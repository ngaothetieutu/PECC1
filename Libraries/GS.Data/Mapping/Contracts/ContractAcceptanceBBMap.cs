using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    class ContractAcceptanceBBMap : GSEntityTypeConfiguration<ContractAcceptanceBB>
    {
        public override void Configure(EntityTypeBuilder<ContractAcceptanceBB> builder)
        {
            builder.ToTable(nameof(ContractAcceptanceBB));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.TaskId).IsRequired();
            builder.Property(t => t.ContractId).IsRequired();
            builder.Property(t => t.ContractIdBB).IsRequired();
            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();
            builder.HasOne(t => t.approver)
               .WithMany()
               .HasForeignKey(t => t.ApprovalId);
            builder.HasOne(t => t.currency)
               .WithMany()
               .HasForeignKey(t => t.CurrencyId);
            builder.HasOne(t => t.contract)
               .WithMany()
               .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.contractBB)
                .WithMany()
                .HasForeignKey(t => t.ContractIdBB);
            builder.HasOne(t => t.unit)
                .WithMany()
                .HasForeignKey(t => t.UnitId);
            builder.HasOne(t => t.task)
                .WithMany()
                .HasForeignKey(t => t.TaskId);
            base.Configure(builder);
        }
    }
}
