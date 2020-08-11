using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
   public class ContractPaymentRequestMap : GSEntityTypeConfiguration<ContractPaymentRequest>
    {
        public override void Configure(EntityTypeBuilder<ContractPaymentRequest> builder)
        {
            builder.ToTable(nameof(ContractPaymentRequest));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId)
             .IsRequired();

            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

            builder.HasOne(t => t.contractPaymentPlan)
             .WithMany()
             .HasForeignKey(t => t.PaymentPlanId)
             .IsRequired();

            builder.Ignore(t => t.paymentType);

            base.Configure(builder);
        }
    }
}
