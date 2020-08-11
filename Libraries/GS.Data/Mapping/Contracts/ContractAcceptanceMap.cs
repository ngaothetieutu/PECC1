using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractAcceptanceMap : GSEntityTypeConfiguration<ContractAcceptance>
    {
        public override void Configure(EntityTypeBuilder<ContractAcceptance> builder)
        {
            builder.ToTable(nameof(ContractAcceptance));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Code).IsRequired();            
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
            builder.HasOne(t => t.responsible)
              .WithMany()
              .HasForeignKey(t => t.ResponsibleId);
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.contractPaymentAcceptance)
            .WithMany()
            .HasForeignKey(t => t.PaymentAcceptanceId);

            builder.HasOne(t => t.unit)
            .WithMany()
            .HasForeignKey(t => t.UnitId);
            builder.HasOne(t => t.paymentAdvance)
            .WithMany()
            .HasForeignKey(t => t.PaymentAdvanceId);

            //cac truong bo qua trong dinh nghia csdl           
            builder.Ignore(t => t.contractAcceptanceStatus);           
            base.Configure(builder);
        }
    }
}
