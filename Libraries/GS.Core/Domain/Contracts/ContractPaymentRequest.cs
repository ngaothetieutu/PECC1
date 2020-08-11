using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
   public class ContractPaymentRequest : BaseEntity
    {
        public ContractPaymentRequest()
        {
            this.paymentType = ContractPaymentType.Advance;
        }
        public int TypeId { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractId { get; set; }
        public int PaymentPlanId { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string DataInfo { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TaxRatio { get; set; }

        public virtual Contract contract { get; set; }
        public virtual ContractPaymentPlan contractPaymentPlan { get; set; }
        public virtual Customer creator { get; set; }
        public bool isDeleted { get; set; }
    }
}
