using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractView : BaseEntity
    {
        public Int32 ContractId {get;set;}
        public Int32 CustomerId { get; set; }
        public DateTime ViewDate { get; set; }
        public virtual Contract contract { get; set; }
    }
}
