using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class PaymentExpenditureMap : GSEntityTypeConfiguration<PaymentExpenditure>
    {
        public override void Configure(EntityTypeBuilder<PaymentExpenditure> builder)
        {
            builder.ToTable(nameof(PaymentExpenditure));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.TotalAmount).IsRequired();
            builder.Property(t => t.TypeId).IsRequired();

            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();
            builder.HasOne(t => t.unit)
             .WithMany()
             .HasForeignKey(t => t.UnitId);
            builder.HasOne(t => t.paymentAcceptance)
             .WithMany()
             .HasForeignKey(t => t.PaymentAdvanceId);
            builder.HasOne(t => t.acceptance)
             .WithMany()
             .HasForeignKey(t => t.AcceptanceId);
            builder.HasOne(t => t.currency)
            .WithMany()
            .HasForeignKey(t => t.CurrencyId);
            base.Configure(builder);
        }
    }
}
