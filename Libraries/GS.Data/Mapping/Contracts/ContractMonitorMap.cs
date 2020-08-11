using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GS.Data.Mapping.Contracts
{
    public class ContractMonitorMap : GSEntityTypeConfiguration<ContractMonitor>
    {
        public override void Configure(EntityTypeBuilder<ContractMonitor> builder)
        {
            builder.ToTable(nameof(ContractMonitor));

            builder.HasKey(t => t.Id);
            builder.Property(t => t.ContractId).IsRequired();

            base.Configure(builder);
        }
    }
}
