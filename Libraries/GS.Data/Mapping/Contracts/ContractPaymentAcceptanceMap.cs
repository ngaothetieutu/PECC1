using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractPaymentAcceptanceMap : GSEntityTypeConfiguration<ContractPaymentAcceptance>
    {
        public override void Configure(EntityTypeBuilder<ContractPaymentAcceptance> builder)
        {
            builder.ToTable(nameof(ContractPaymentAcceptance));
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
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.contractPaymentRequest)
            .WithMany()
            .HasForeignKey(t => t.PaymentRequestId);
            //cac truong bo qua trong dinh nghia csdl           
            builder.Ignore(t => t.contractPaymentAcceptancestatus);            
            base.Configure(builder);
        }
    }
}
