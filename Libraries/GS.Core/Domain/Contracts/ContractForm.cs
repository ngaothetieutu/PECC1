using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractForm:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
