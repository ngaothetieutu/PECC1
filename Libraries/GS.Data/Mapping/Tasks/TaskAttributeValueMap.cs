using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Tasks;

namespace GS.Data.Mapping.Tasks
{
    public partial class TaskAttributeValueMap : GSEntityTypeConfiguration<TaskAttributeValue>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<TaskAttributeValue> builder)
        {
            builder.ToTable(nameof(TaskAttributeValue));
            builder.HasKey(value => value.Id);

            builder.Property(value => value.Name).HasMaxLength(400).IsRequired();

            builder.HasOne(value => value.TaskAttribute)
                .WithMany(attribute => attribute.TaskAttributeValues)
                .HasForeignKey(value => value.TaskAttributeId)
                .IsRequired();

            base.Configure(builder);
        }
        #endregion
    }
}
