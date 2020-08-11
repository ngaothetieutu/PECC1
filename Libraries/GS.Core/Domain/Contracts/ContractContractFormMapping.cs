using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractContractFormMapping : BaseEntity
    {
        public int ContractId { get; set; }

        public int ContractFormId { get; set; }

        public virtual Contract contract { get; set; }

        public virtual ContractForm contractForm { get; set; }
    }
}
