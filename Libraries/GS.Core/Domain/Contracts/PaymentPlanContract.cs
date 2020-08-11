using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class PaymentPlanContract : BaseEntity
    {
        public int PaymentPlanId { get; set; }
        public int ContractId { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Contract contract {get; set;}
        public virtual ContractPaymentPlan Paymentplan {get; set;}
    }
}
