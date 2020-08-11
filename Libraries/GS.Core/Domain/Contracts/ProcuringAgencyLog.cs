using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ProcuringAgencyLog : BaseEntity
    {
        public Int32 ProcuringAgencyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public String ProcuringAgencyData { get; set; }
        public virtual ProcuringAgency procuringAgency { get; set; }
    }
}
