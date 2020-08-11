using GS.Core.Domain.Tasks;

namespace GS.Core.Domain.Security
{
    public partial class PermissionRecordTaskRoleMapping : BaseEntity
    {
        public int PermissionRecordId { get; set; }

        public int TaskRoleId { get; set; }

        public virtual PermissionRecord PermissionRecord { get; set; }

        public virtual TaskRole TaskRole { get; set; }
    }
}
