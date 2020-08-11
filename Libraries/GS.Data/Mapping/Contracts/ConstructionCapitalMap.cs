using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ConstructionCapitalMap : GSEntityTypeConfiguration<ConstructionCapital>
    {
        public override void Configure(EntityTypeBuilder<ConstructionCapital> builder)
        {
            builder.ToTable(nameof(ConstructionCapital));
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            base.Configure(builder);
        }
    }
}
