using System.Collections.Generic;
using GS.Core.Domain.Security;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractRole : BaseEntity
    {
        private ICollection<PermissionRecordContractRoleMapping> _permissionRecordContractRoleMappings;

        public string Name { get; set; }

        public bool FreeShipping { get; set; }

        public bool TaxExempt { get; set; }

        public bool Active { get; set; }

        public bool IsSystemRole { get; set; }

        public string SystemName { get; set; }

        public bool EnablePasswordLifetime { get; set; }

        public bool OverrideTaxDisplayType { get; set; }

        public int DefaultTaxDisplayTypeId { get; set; }

        public int PurchasedWithProductId { get; set; }

        public virtual ICollection<PermissionRecordContractRoleMapping> PermissionRecordContractRoleMappings
        {
            get => _permissionRecordContractRoleMappings ?? (_permissionRecordContractRoleMappings = new List<PermissionRecordContractRoleMapping>());
            protected set => _permissionRecordContractRoleMappings = value;
        }
    }
}
