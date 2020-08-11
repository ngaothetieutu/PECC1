using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ConstructionMap : GSEntityTypeConfiguration<Construction>
    {
        public override void Configure(EntityTypeBuilder<Construction> builder)
        {
            builder.ToTable(nameof(Construction));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code).IsRequired();
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).IsRequired();

            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.constructionType)
               .WithMany()
               .HasForeignKey(t => t.TypeId);
            builder.HasOne(t => t.constructionCapital)
              .WithMany()
              .HasForeignKey(t => t.CapitalId);

            builder.Ignore(t => t.ContractCount);

            builder.Ignore(t => t.TotalMoneyContract);

            base.Configure(builder);
        }
    }
}
