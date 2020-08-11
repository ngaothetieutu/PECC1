using GS.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.Localization;

namespace GS.Core.Domain.Contracts
{
    public class ContractAttribute : BaseEntity, ILocalizedEntity
    {
        private ICollection<ContractAttributeValue> _contractAttributeValues;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the attribute control type identifier
        /// </summary>
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        public AttributeControlType AttributeControlType
        {
            get => (AttributeControlType)AttributeControlTypeId;
            set => AttributeControlTypeId = (int)value;
        }

        public virtual ICollection<ContractAttributeValue> ContractAttributeValues
        {
            get => _contractAttributeValues ?? (_contractAttributeValues = new List<ContractAttributeValue>());
            protected set => _contractAttributeValues = value;
        }
    }
}
