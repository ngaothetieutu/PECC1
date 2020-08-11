using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractContractTypeMapping : BaseEntity
    {
        public int ContractId { get; set; }

        public int ContractTypeId { get; set; }

        public virtual Contract contract { get; set; }

        public virtual ContractType contractType { get; set; }
    }
}
