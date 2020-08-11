using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum PaymentAcceptanceSubStatus
    {
        /// <summary>
        /// Su dung
        /// </summary>
        Use = 1,
        /// <summary>
        /// Xoa
        /// </summary>
        Destroy = 4
    }
    public partial class ContractPaymentAcceptanceSub : BaseEntity
    {
        public ContractPaymentAcceptanceSub()
        {
            StatusId = (int)PaymentAcceptanceSubStatus.Use;
        }            
        public int? AcceptanceId { get; set; }
        public int ContractId { get; set; }
        public Int32? UnitId { get; set; }
        public int TaskId { get; set; }
        public int PaymentAcceptanceId { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalMoney { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32? CurrencyId { get; set; }
        public Int32 CreatorId { get; set; }
        public decimal? ReduceAdvance { get; set; }
        public decimal? ReduceKeep { get; set; }
        public decimal? ReduceOther { get; set; }
        public decimal? Depreciation { get; set; }
        public virtual ContractPaymentAcceptance contractPaymentAcceptance { get; set; }
        public virtual Customer creator { get; set; }
        public virtual Contract contract { get; set; }
        public virtual Unit unit { get; set; }
        public virtual ContractAcceptance contractAcceptance { get; set; }
        public virtual Currency currency { get; set; }
        public virtual Task task { get; set; }
    }
}
