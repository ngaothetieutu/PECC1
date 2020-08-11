using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
   public partial class ContractPaymentTaskMapping : BaseEntity
    {
        public int TaskId { get; set; }
        public int ContractPaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual ContractPayment contractPayment { get; set; }
    }
}
