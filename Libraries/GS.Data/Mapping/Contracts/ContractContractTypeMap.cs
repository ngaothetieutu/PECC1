using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace GS.Data.Mapping.Contracts
{

    public partial class ContractContractTypeMap : GSEntityTypeConfiguration<ContractContractTypeMapping>
    {
        public override void Configure(EntityTypeBuilder<ContractContractTypeMapping> builder)
        {
            builder.ToTable("Contract_ContractType_Mapping");
            builder.HasKey(mapping => new { mapping.ContractId, mapping.ContractTypeId });

            builder.Property(mapping => mapping.ContractId).HasColumnName("Contract_Id");
            builder.Property(mapping => mapping.ContractTypeId).HasColumnName("ContractType_Id");

            builder.HasOne(mapping => mapping.contract)
                .WithMany(c => c.ContractContractTypeMappings)
                .HasForeignKey(mapping => mapping.ContractId)
                .IsRequired();

            builder.HasOne(mapping => mapping.contractType)
                .WithMany()
                .HasForeignKey(mapping => mapping.ContractTypeId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
