using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
   
    public class ContractPaymentPlan : BaseEntity
    {
        public ContractPaymentPlan()
        {
            paymentType = ContractPaymentType.Advance;
        }
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public String Description { get; set; }
        public String PayRule { get; set; }
        public decimal Amount { get; set; }
        public decimal PercentNum { get; set; }
        public int TypeId { get; set; }
        public string AmountSummary { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
    }
}
