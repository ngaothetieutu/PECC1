using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskResourceMap : GSEntityTypeConfiguration<TaskResource>
    {
        public override void Configure(EntityTypeBuilder<TaskResource> builder)
        {
            builder.ToTable(nameof(TaskResource));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.task)
               .WithMany()
               .HasForeignKey(t => t.TaskId)
               .IsRequired();

            builder.HasOne(t => t.customer)
             .WithMany()
             .HasForeignKey(t => t.CustomerId)
             .IsRequired();

            builder.HasOne(t => t.contractRole)
             .WithMany()
             .HasForeignKey(t => t.RoleId)
             .IsRequired();
            builder.HasOne(t => t.unitInfo)
              .WithMany()
              .HasForeignKey(t => t.UnitId);

            base.Configure(builder);
        }
    }
}
