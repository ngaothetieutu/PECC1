using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Vendors;

namespace GS.Data.Mapping.Vendors
{
    /// <summary>
    /// Represents a vendor attribute value mapping configuration
    /// </summary>
    public partial class VendorAttributeValueMap : GSEntityTypeConfiguration<VendorAttributeValue>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<VendorAttributeValue> builder)
        {
            builder.ToTable(nameof(VendorAttributeValue));
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();

            builder.HasOne(value => value.VendorAttribute)
                .WithMany(attribute => attribute.VendorAttributeValues)
                .HasForeignKey(value => value.VendorAttributeId)
                .IsRequired();

            base.Configure(builder);
        }
        
        #endregion
    }
}