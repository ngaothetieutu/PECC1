using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractResource : BaseEntity
    {
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer customer { get; set; }
        public Int32 RoleId { get; set; }
        public virtual CustomerRole contractRole { get; set; }
        public String Note { get; set; }
        public int? UnitId { get; set; }

        public virtual Unit unitInfo { get; set; }
        public int? GroupId { get; set; }
        public virtual CustomerRole groupRole { get; set; }
    }
}
