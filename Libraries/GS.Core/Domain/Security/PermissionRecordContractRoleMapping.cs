using GS.Core.Domain.Contracts;

namespace GS.Core.Domain.Security
{
    public partial class PermissionRecordContractRoleMapping : BaseEntity
    {
        public int PermissionRecordId { get; set; }

        public int CustomerRoleId { get; set; }

        public virtual PermissionRecord PermissionRecord { get; set; }

        public virtual ContractRole CustomerRole { get; set; }
    }
}
