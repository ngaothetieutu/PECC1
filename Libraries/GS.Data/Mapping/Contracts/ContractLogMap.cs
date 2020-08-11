using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace GS.Data.Mapping.Contracts
{
    public class ContractLogMap : GSEntityTypeConfiguration<ContractLog>
    {
        public override void Configure(EntityTypeBuilder<ContractLog> builder)
        {
            builder.ToTable(nameof(ContractLog));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

            builder.HasOne(t => t.period)
               .WithMany()
               .HasForeignKey(t => t.PeriodId);

            base.Configure(builder);
        }
    }
}
