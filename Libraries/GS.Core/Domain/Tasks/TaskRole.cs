using System.Collections.Generic;
using GS.Core.Domain.Security;

namespace GS.Core.Domain.Tasks
{
    public partial class TaskRole : BaseEntity
    {
        private ICollection<PermissionRecordTaskRoleMapping> _permissionRecordTaskRoleMappings;

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

        public virtual ICollection<PermissionRecordTaskRoleMapping> PermissionRecordTaskRoleMappings
        {
            get => _permissionRecordTaskRoleMappings ?? (_permissionRecordTaskRoleMappings = new List<PermissionRecordTaskRoleMapping>());
            protected set => _permissionRecordTaskRoleMappings = value;
        }
    }
}
