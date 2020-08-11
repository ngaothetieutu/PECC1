using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Customers;

namespace GS.Data.Mapping.Customers
{
    /// <summary>
    /// Represents a customer attribute mapping configuration
    /// </summary>
    public partial class CustomerAttributeMap : GSEntityTypeConfiguration<CustomerAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerAttribute> builder)
        {
            builder.ToTable(nameof(CustomerAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }

        #endregion
    }
}