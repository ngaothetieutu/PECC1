using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractLog : BaseEntity
    {
        public Int32 ContractId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public String Note { get; set; }
        public String ContractData { get; set; }
        public Int32? PeriodId { get; set; }
        public virtual ContractPeriod period { get; set; }
    }
}
