using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public class TaskResource : BaseEntity
    {
        public Int32 TaskId { get; set; }
        public virtual Task task { get; set; }
        public Int32 CustomerId { get; set; }
        public virtual Customer customer { get; set; }
        public Int32 RoleId { get; set; }
        public virtual CustomerRole contractRole { get; set; }
        public String Note { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit unitInfo { get; set; }
    }
}
