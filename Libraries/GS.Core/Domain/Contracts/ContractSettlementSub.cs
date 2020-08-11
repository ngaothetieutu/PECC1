using GS.Core.Domain.Customers;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractSettlementSub : BaseEntity
    {
        public int ContractId { get; set; }
        public int TaskId { get; set; }
        public int ContractSettlementId { get; set; }
        public Decimal TotalAmount { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ContractSettlement ContractSettlement { get; set; }
        public virtual Task Tasks { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Customer Creator { get; set; }
    }
}
