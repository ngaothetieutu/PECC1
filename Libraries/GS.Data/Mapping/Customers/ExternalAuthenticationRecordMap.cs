using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Customers;

namespace GS.Data.Mapping.Customers
{
    /// <summary>
    /// Represents an external authentication record mapping configuration
    /// </summary>
    public partial class ExternalAuthenticationRecordMap : GSEntityTypeConfiguration<ExternalAuthenticationRecord>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ExternalAuthenticationRecord> builder)
        {
            builder.ToTable(nameof(ExternalAuthenticationRecord));
            builder.HasKey(record => record.Id);

            builder.HasOne(record => record.Customer)
                .WithMany(customer => customer.ExternalAuthenticationRecords)
                .HasForeignKey(record => record.CustomerId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}