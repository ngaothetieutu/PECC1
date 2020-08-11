using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Tasks;

namespace GS.Data.Mapping.Tasks
{
    public partial class TaskAttributeMap : GSEntityTypeConfiguration<TaskAttribute>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<TaskAttribute> builder)
        {
            builder.ToTable(nameof(TaskAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(400).IsRequired();

            builder.Ignore(attribute => attribute.AttributeControlType);

            base.Configure(builder);
        }
        #endregion
    }
}
