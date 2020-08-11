using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractContractPeriodMapping : BaseEntity
    {
        public int ContractId { get; set; }

        public int ContractPeriodId { get; set; }

        public virtual Contract contract { get; set; }

        public virtual ContractPeriod contractPeriod { get; set; }
    }
}

