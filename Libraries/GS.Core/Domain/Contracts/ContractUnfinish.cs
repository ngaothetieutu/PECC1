using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractUnfinish : BaseEntity
    {
        public Int32 ContractId {get;set;}        
        public DateTime CreatedDate { get; set; }
        public virtual Contract contract { get; set; }
        public Int32 ContractTypeId { get; set; }
        public decimal? AutoValue { get; set; }
        public decimal? OptionValue { get; set; }       
        public int? CreatorId { get; set; }
        public int? TypeId { get; set; }
        public virtual Customer customer { get; set; }
        public virtual ContractType contractType { get; set; }
    }
}
