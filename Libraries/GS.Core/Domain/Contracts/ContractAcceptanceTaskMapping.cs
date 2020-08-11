using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractAcceptanceTaskMapping : BaseEntity
    {       
        public int TaskId { get; set; }

        public int ContractAcceptanceId { get; set; }

        public virtual Task task { get; set; }

        public virtual ContractAcceptance contractAcceptance { get; set; }
    }
}
