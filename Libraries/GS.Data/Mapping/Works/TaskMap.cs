using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskMap : GSEntityTypeConfiguration<Task>
    {
        public override void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable(nameof(Task));
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();

            builder.HasOne(t => t.contract)
            .WithMany()
            .HasForeignKey(t => t.ContractId)
            .IsRequired();

            builder.HasOne(t => t.parentTask)
           .WithMany()
           .HasForeignKey(t => t.ParentId);

            builder.HasOne(t => t.approver)
               .WithMany()
               .HasForeignKey(t => t.ApproverId);

            builder.HasOne(t => t.preTask)
             .WithMany()
             .HasForeignKey(t => t.PreTaskId);

            builder.HasOne(t => t.responsible)
              .WithMany()
              .HasForeignKey(t => t.ResponsibleId);

            builder.HasOne(t => t.measureDimension)
            .WithMany()
            .HasForeignKey(t => t.MeasureId);


            builder.HasOne(t => t.currency)
               .WithMany()
               .HasForeignKey(t => t.CurrencyId);

            builder.HasOne(t => t.unitInfo)
               .WithMany()
               .HasForeignKey(t => t.UnitId);

            builder.HasOne(t => t.taskGroup)
                .WithMany()
                .HasForeignKey(t => t.TaskGroupId);
            builder.HasOne(t => t.contractType)
               .WithMany()
               .HasForeignKey(t => t.ContractTypeId);
            builder.HasOne(t => t.procuringAgency)
               .WithMany()
               .HasForeignKey(t => t.TaskProcuringAgencyId);

            builder.Ignore(t => t.taskStatus);
            builder.Ignore(t => t.taskLevelId);
            base.Configure(builder);
        }
    }
}
