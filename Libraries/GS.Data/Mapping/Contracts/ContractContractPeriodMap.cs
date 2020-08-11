using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
   public partial class ContractContractPeriodMap : GSEntityTypeConfiguration<ContractContractPeriodMapping>
    {
        public override void Configure(EntityTypeBuilder<ContractContractPeriodMapping> builder)
        {
            builder.ToTable("Contract_ContractPeriod_Mapping");
            builder.HasKey(mapping => new { mapping.ContractId, mapping.ContractPeriodId });

            builder.Property(mapping => mapping.ContractId).HasColumnName("Contract_Id");
            builder.Property(mapping => mapping.ContractPeriodId).HasColumnName("ContractPeriod_Id");

            builder.HasOne(mapping => mapping.contract)
                .WithMany(c => c.ContractContractPeriodMappings)
                .HasForeignKey(mapping => mapping.ContractId)
                .IsRequired();

            builder.HasOne(mapping => mapping.contractPeriod)
                .WithMany()
                .HasForeignKey(mapping => mapping.ContractPeriodId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
