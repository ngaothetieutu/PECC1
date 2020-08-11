using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Contracts;

namespace GS.Data.Mapping.Contracts
{
    public partial class ContractAttributeValueMap : GSEntityTypeConfiguration<ContractAttributeValue>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ContractAttributeValue> builder)
        {
            builder.ToTable(nameof(ContractAttributeValue));
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();

            builder.HasOne(value => value.ContractAttribute)
                .WithMany(attribute => attribute.ContractAttributeValues)
                .HasForeignKey(value => value.ContractAttributeId)
                .IsRequired();

            base.Configure(builder);
        }
        #endregion
    }
}
