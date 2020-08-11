using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskGroupMap: GSEntityTypeConfiguration<TaskGroup>
    {
        public override void Configure(EntityTypeBuilder<TaskGroup> builder)
        {
            builder.ToTable(nameof(TaskGroup));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
