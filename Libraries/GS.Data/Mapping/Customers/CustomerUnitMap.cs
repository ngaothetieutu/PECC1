using GS.Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Customers
{
    public class CustomerUnitMap: GSEntityTypeConfiguration<CustomerUnitMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerUnitMapping> builder)
        {
            builder.ToTable(GSMappingDefaults.CustomerUnitTable);
            builder.HasKey(mapping => new { mapping.CustomerId, mapping.UnitId });

            builder.Property(mapping => mapping.CustomerId).HasColumnName("Customer_Id");
            builder.Property(mapping => mapping.UnitId).HasColumnName("Unit_Id");

            builder.HasOne(mapping => mapping.Customer)
                .WithMany(customer => customer.CustomerUnitMappings)
                .HasForeignKey(mapping => mapping.CustomerId)
                .IsRequired();

            builder.HasOne(mapping => mapping.Unit)
                .WithMany()
                .HasForeignKey(mapping => mapping.UnitId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }

        #endregion
    }
}
