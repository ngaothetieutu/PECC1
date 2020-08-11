using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Customers;

namespace GS.Data.Mapping.Customers
{
    /// <summary>
    /// Represents a customer password mapping configuration
    /// </summary>
    public partial class CustomerPasswordMap : GSEntityTypeConfiguration<CustomerPassword>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerPassword> builder)
        {
            builder.ToTable(nameof(CustomerPassword));
            builder.HasKey(password => password.Id);

            builder.HasOne(password => password.Customer)
                .WithMany()
                .HasForeignKey(password => password.CustomerId)
                .IsRequired();

            builder.Ignore(password => password.PasswordFormat);

            base.Configure(builder);
        }

        #endregion
    }
}