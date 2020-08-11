using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Customers
{
    public class CustomerUnitMapping: BaseEntity
    {
        public int CustomerId { get; set; }
        public int UnitId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Unit Unit { get; set; }

    }
}
