using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace GS.Data.Mapping.Contracts
{
    public partial class ContractContractFormMap : GSEntityTypeConfiguration<ContractContractFormMapping>
    {
        public override void Configure(EntityTypeBuilder<ContractContractFormMapping> builder)
        {
            builder.ToTable("Contract_ContractForm_Mapping");
            builder.HasKey(mapping => new { mapping.ContractId, mapping.ContractFormId });

            builder.Property(mapping => mapping.ContractId).HasColumnName("Contract_Id");
            builder.Property(mapping => mapping.ContractFormId).HasColumnName("ContractForm_Id");

            builder.HasOne(mapping => mapping.contract)
                .WithMany(c => c.ContractContractFormMappings)
                .HasForeignKey(mapping => mapping.ContractId)
                .IsRequired();

            builder.HasOne(mapping => mapping.contractForm)
                .WithMany()
                .HasForeignKey(mapping => mapping.ContractFormId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
