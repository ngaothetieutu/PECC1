using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class Construction : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public int? CapitalId { get; set; }

        public virtual ConstructionType constructionType { get; set; }
        public virtual ConstructionCapital constructionCapital { get; set; }

        public int ContractCount { get; set; }
        public string TotalMoneyContract { get; set; }
    }
}
