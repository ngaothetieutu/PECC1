using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractPaymentTaskMap : GSEntityTypeConfiguration<ContractPaymentTask>
    {
        public override void Configure(EntityTypeBuilder<ContractPaymentTask> builder)
        {
            builder.ToTable(nameof(ContractPaymentTask));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contractPayment)
             .WithMany()
             .HasForeignKey(t => t.PaymentId)
             .IsRequired();
            builder.HasOne(t => t.taskInfo)
               .WithMany()
               .HasForeignKey(t => t.TaskId)
               .IsRequired();
            builder.HasOne(t => t.currency)
                .WithMany()
                .HasForeignKey(t => t.CurrencyId)
                .IsRequired();
            base.Configure(builder);
        }
    }
}
