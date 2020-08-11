using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public class TaskContract : BaseEntity
    {
        public Int32 TaskId { get; set; }
        public Int32 ContractId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public string Note { get; set; }
        public virtual Contract contract { get; set; }
        public virtual Customer creator { get; set; }
        public virtual Task task { get; set; }

    }
}
