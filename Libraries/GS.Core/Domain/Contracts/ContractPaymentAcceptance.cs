using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    /// <summary>
    /// 1: draf, 2: approved, 4: destroy
    /// </summary>
    public enum ContractPaymentAcceptanceStatus
    {
        All = 0,
        /// <summary>
        /// Nháp
        /// </summary>
        Draf = 1,
        /// <summary>
        /// Cho duyệt
        /// </summary>
        Pending = 2,
        /// <summary>
        /// Đã duyệt
        /// </summary>
        Approved = 3,
        /// <summary>
        /// Hủy
        /// </summary>
        Destroy = 4
    }
    public class ContractPaymentAcceptance : BaseEntity
    {
        public ContractPaymentAcceptance()
        {
            this.contractPaymentAcceptancestatus = ContractPaymentAcceptanceStatus.Draf;
        }
        public String Name { get; set; }
        public String Code { get; set; }
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public Int32? ApprovalId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public virtual Customer approver { get; set; }
        public Int32 StatusId { get; set; }
        public Decimal? TotalAmount { get; set; }
        public ContractPaymentAcceptanceStatus contractPaymentAcceptancestatus
        {
            get => (ContractPaymentAcceptanceStatus)StatusId;
            set => StatusId = (int)value;
        }
        public Int32? PaymentRequestId { get; set; }
        public virtual ContractPaymentRequest contractPaymentRequest { get; set; }       
        public Int32? CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        public String DataInfo { get; set; }
        public Decimal ReduceAdvance { get; set; }
        public Decimal ReduceKeep { get; set; }
        public Decimal ReduceOther { get; set; }
        public Decimal? Depreciation { get; set; }
        
    }
}
