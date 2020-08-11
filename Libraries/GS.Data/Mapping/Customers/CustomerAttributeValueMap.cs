using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Customers;

namespace GS.Data.Mapping.Customers
{
    /// <summary>
    /// Represents a customer attribute value mapping configuration
    /// </summary>
    public partial class CustomerAttributeValueMap : GSEntityTypeConfiguration<CustomerAttributeValue>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerAttributeValue> builder)
        {
            builder.ToTable(nameof(CustomerAttributeValue));
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();

            builder.HasOne(value => value.CustomerAttribute)
                .WithMany(attribute => attribute.CustomerAttributeValues)
                .HasForeignKey(value => value.CustomerAttributeId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}