using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace GS.Data.Mapping.Contracts
{
    public partial class ContractAcceptanceTaskMap : GSEntityTypeConfiguration<ContractAcceptanceTaskMapping>
    {
        public override void Configure(EntityTypeBuilder<ContractAcceptanceTaskMapping> builder)
        {
            builder.ToTable("ContractAcceptance_Task_Mapping");
            builder.HasKey(mapping => new { mapping.TaskId, mapping.ContractAcceptanceId });

            builder.Property(mapping => mapping.TaskId).HasColumnName("TaskId");
            builder.Property(mapping => mapping.ContractAcceptanceId).HasColumnName("ContractAcceptanceId");
            builder.HasOne(mapping => mapping.task)
                .WithMany()
                .HasForeignKey(mapping => mapping.TaskId)
                .IsRequired();
            builder.HasOne(mapping => mapping.contractAcceptance)
                .WithMany()
                .HasForeignKey(mapping => mapping.ContractAcceptanceId)
                .IsRequired();

            builder.Ignore(mapping => mapping.Id);

            base.Configure(builder);
        }
    }
}
