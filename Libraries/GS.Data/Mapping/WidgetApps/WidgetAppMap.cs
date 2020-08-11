using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.WidgetApps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.WidgetApps
{
    public class WidgetAppMap : GSEntityTypeConfiguration<WidgetApp>
    {
        public override void Configure(EntityTypeBuilder<WidgetApp> builder)
        {
            builder.ToTable(nameof(WidgetApp));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.ViewId).IsRequired();
        }
    }
}
