using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace GS.Data.Mapping.Contracts
{
    public partial class ContractSettlementTaskMap : GSEntityTypeConfiguration<ContractSettlementTaskMapping>
    {
        public override void Configure(EntityTypeBuilder<ContractSettlementTaskMapping> builder)
        {
            builder.ToTable("ContractSettlement_Task_Mapping");
            builder.HasKey(mapping => new { mapping.SettlementId, mapping.TaskId });

            builder.Property(mapping => mapping.SettlementId).HasColumnName("SettlementId");
            builder.Property(mapping => mapping.TaskId).HasColumnName("TaskId");

            builder.HasOne(mapping => mapping.tasks)
                .WithMany(c => c.ContractSettlementTaskMappings)
                .HasForeignKey(mapping => mapping.TaskId)
                .IsRequired();

            builder.HasOne(mapping => mapping.contractSettlement)
                .WithMany()
                .HasForeignKey(mapping => mapping.SettlementId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
