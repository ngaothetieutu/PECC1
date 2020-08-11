using GS.Core.Domain.Localization;

namespace GS.Core.Domain.Tasks
{
    public partial class TaskAttributeValue : BaseEntity, ILocalizedEntity
    {
        public int TaskAttributeId { get; set; }

        public string Name { get; set; }

        public bool IsPreSelected { get; set; }

        public int DisplayOrder { get; set; }

        public virtual TaskAttribute TaskAttribute { get; set; }
    }
}
