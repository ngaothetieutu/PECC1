using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Gdpr;

namespace GS.Data.Mapping.Gdpr
{
    /// <summary>
    /// Represents a GDPR consent mapping configuration
    /// </summary>
    public partial class GdprConsentMap : GSEntityTypeConfiguration<GdprConsent>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<GdprConsent> builder)
        {
            builder.ToTable(nameof(GdprConsent));
            builder.HasKey(gdpr => gdpr.Id);

            builder.Property(category => category.Message).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}