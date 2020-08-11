using GS.Core.Domain.Works;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class WorkFileMap : GSEntityTypeConfiguration<WorkFile>
    {
        public override void Configure(EntityTypeBuilder<WorkFile> builder)
        {
            builder.ToTable(nameof(WorkFile));
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

            builder.Ignore(t => t.fileType);
            base.Configure(builder);
        }
    }
}
