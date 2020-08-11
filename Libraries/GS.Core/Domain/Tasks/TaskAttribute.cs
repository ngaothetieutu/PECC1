using System.Collections.Generic;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Localization;

namespace GS.Core.Domain.Tasks
{
    public partial class TaskAttribute : BaseEntity, ILocalizedEntity
    {
        private ICollection<TaskAttributeValue> _taskAttributeValues;

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public int AttributeControlTypeId { get; set; }

        public int DisplayOrder { get; set; }

        public AttributeControlType AttributeControlType
        {
            get => (AttributeControlType)AttributeControlTypeId;
            set => AttributeControlTypeId = (int)value;
        }

        public virtual ICollection<TaskAttributeValue> TaskAttributeValues
        {
            get => _taskAttributeValues ?? (_taskAttributeValues = new List<TaskAttributeValue>());
            protected set => _taskAttributeValues = value;
        }
    }
}
