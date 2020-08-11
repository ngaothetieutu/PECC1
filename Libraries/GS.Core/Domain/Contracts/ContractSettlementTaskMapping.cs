using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractSettlementTaskMapping : BaseEntity
    {
        public int TaskId { get; set; }

        public int SettlementId { get; set; }

        public virtual ContractSettlement contractSettlement { get; set; }

        public virtual Task tasks { get; set; }
    }
}
