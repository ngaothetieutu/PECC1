using GS.Core.Domain.Localization;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractAttributeValue : BaseEntity, ILocalizedEntity
    {
        public int ContractAttributeId { get; set; }

        public string Name { get; set; }

        public bool IsPreSelected { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ContractAttribute ContractAttribute { get; set; }
    }
}
