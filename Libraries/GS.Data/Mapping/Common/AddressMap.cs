﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Common;

namespace GS.Data.Mapping.Common
{
    /// <summary>
    /// Represents an address mapping configuration
    /// </summary>
    public partial class AddressMap : GSEntityTypeConfiguration<Address>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable(nameof(Address));
            builder.HasKey(address => address.Id);

            builder.HasOne(address => address.Country)
                .WithMany()
                .HasForeignKey(address => address.CountryId);

            builder.HasOne(address => address.StateProvince)
                .WithMany()
                .HasForeignKey(address => address.StateProvinceId);

            base.Configure(builder);
        }

        #endregion
    }
}