using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Advance;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.AdvancePayment
{
    public class PaymentAdvanceMap : GSEntityTypeConfiguration<ContractPaymentAdvance>
    {
        public override void Configure(EntityTypeBuilder<ContractPaymentAdvance> builder)
        {
            builder.ToTable(nameof(ContractPaymentAdvance));
            builder.HasKey(t => t.Id);           
            builder.Property(t => t.Name).IsRequired();
            builder.HasOne(t => t.unit)
             .WithMany()
             .HasForeignKey(t => t.UnitId);
            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId);
            builder.HasOne(t => t.currency)
             .WithMany()
             .HasForeignKey(t => t.CurrencyId);
            base.Configure(builder);           
        }
    }
}
