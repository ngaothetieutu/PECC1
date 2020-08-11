using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskFileMap : GSEntityTypeConfiguration<TaskFile>
    {
        public override void Configure(EntityTypeBuilder<TaskFile> builder)
        {
            builder.ToTable(nameof(TaskFile));
            builder.HasKey(t => t.Id);

            base.Configure(builder);
        }
    }
}
