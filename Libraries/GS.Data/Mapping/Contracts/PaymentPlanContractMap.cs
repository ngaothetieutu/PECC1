using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class PaymentPlanContractMap : GSEntityTypeConfiguration<PaymentPlanContract>
    {
        public override void Configure(EntityTypeBuilder<PaymentPlanContract> builder)
        {
            builder.ToTable("PaymentPlan_Contract");
            builder.HasKey(mapping => new { mapping.ContractId, mapping.PaymentPlanId });
            //builder.HasKey(t => t.Id);
            builder.Property(mapping => mapping.ContractId).HasColumnName("ContractId");
            builder.Property(mapping => mapping.PaymentPlanId).HasColumnName("PaymentPlanId");
            builder.Property(mapping => mapping.Note).HasColumnName("Note");
            builder.Property(mapping => mapping.CreateDate).HasColumnName("CreateDate");
            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId)
             .IsRequired();
            builder.HasOne(t => t.Paymentplan)
             .WithMany()
             .HasForeignKey(t => t.PaymentPlanId)
             .IsRequired();
            builder.Ignore(mapping => mapping.Id);
            base.Configure(builder);
        }
    }
}
