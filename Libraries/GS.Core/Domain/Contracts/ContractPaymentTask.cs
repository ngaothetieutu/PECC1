using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractPaymentTask : BaseEntity
    {
        public Int32 PaymentId { get; set; }
        public virtual ContractPayment contractPayment { get; set; }
        public Int32 TaskId { get; set; }
        public virtual Task taskInfo { get; set; }
        public String Note { get; set; }
        public Decimal Amount { get; set; }
        public Int32 CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
    }
}
