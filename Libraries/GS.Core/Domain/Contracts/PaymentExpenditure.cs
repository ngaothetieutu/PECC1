using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum PaymentExpenditureType
    {
        /// <summary>
        /// Tam ung san luong 
        /// </summary>
        Advance = 1,
        /// <summary>
        /// nghiem thu noi bo
        /// </summary>
        Acceptance = 2,
    }
    public class PaymentExpenditure : BaseEntity
    {
        public PaymentExpenditure()
        {
            this.ExpenditureGuid = Guid.NewGuid();
        }
        public Guid ExpenditureGuid { get; set; }
        public String Name { get; set; }
        public String Code { get; set; }
        public int TypeId { get; set; }
        public String Description { get; set; }
        public int UnitId { get; set; }
        public Decimal TotalAmount { get; set; }
        public int? PaymentAdvanceId { get; set; }
        public int? AcceptanceId { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Unit unit { get; set; }
        public virtual Customer creator { get; set; }
        public virtual ContractPaymentAcceptance paymentAcceptance { get; set; }
        public virtual ContractAcceptance acceptance { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
    }
}
