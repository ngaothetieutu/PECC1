using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskLogMap : GSEntityTypeConfiguration<TaskLog>
    {
        public override void Configure(EntityTypeBuilder<TaskLog> builder)
        {
            builder.ToTable(nameof(TaskLog));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
