using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractFileMap : GSEntityTypeConfiguration<ContractFile>
    {
        public override void Configure(EntityTypeBuilder<ContractFile> builder)
        {
            builder.ToTable(nameof(ContractFile));
            builder.HasKey(t => t.Id);


            builder.HasOne(t => t.workFile)
             .WithMany()
             .HasForeignKey(t => t.FileId)
             .IsRequired();

            builder.Ignore(t => t.contractFileType);

            base.Configure(builder);
        }
    }
}
