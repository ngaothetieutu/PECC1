using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractAttributeMap : GSEntityTypeConfiguration<ContractAttribute>
    {

        #region Methods
        public override void Configure(EntityTypeBuilder<ContractAttribute> builder)
        {
            builder.ToTable(nameof(ContractAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }
        #endregion
    }
}
