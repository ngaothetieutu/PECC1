using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractMap : GSEntityTypeConfiguration<Contract>
    {
        public override void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable(nameof(Contract));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Name).IsRequired();

            //dinh nghia lien ket foreikey
            //builder.HasOne(t => t.period)
            //   .WithMany()
            //   .HasForeignKey(t => t.PeriodId);

            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();

            builder.HasOne(t => t.approver)
               .WithMany()
               .HasForeignKey(t => t.ApproverId);

            builder.HasOne(t => t.construction)
               .WithMany()
               .HasForeignKey(t => t.ConstructionId);

            builder.HasOne(t => t.currency)
               .WithMany()
               .HasForeignKey(t => t.CurrencyId);

            //cac truong bo qua trong dinh nghia csdl
            builder.Ignore(t => t.classification);
            builder.Ignore(t => t.status);
            builder.Ignore(t => t.ContractForms);
            builder.Ignore(t => t.ContractTypes);
            builder.Ignore(t => t.ContractPeriods);

            base.Configure(builder);
        }
    }
}
