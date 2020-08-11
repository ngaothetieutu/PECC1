using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractRelate : BaseEntity
    {
        public Int32 Contract1Id { get; set; }
        public virtual Contract contract1 { get; set; }
        public Int32 Contract2Id { get; set; }
        public virtual Contract contract2 { get; set; }
        public Int32 DisplayOrder { get; set; }
        public String Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
