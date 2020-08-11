using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ContractPaymentStatus
    {
        /// <summary>
        /// Nháp
        /// </summary>
        Draf=1,
        /// <summary>
        /// Cho duyệt
        /// </summary>
        Pending=2,
        /// <summary>
        /// Đã duyệt
        /// </summary>
        Approved=3,
        /// <summary>
        /// Hủy
        /// </summary>
        Destroy=4
    }
    public enum ContractPaymentType
    {
        /// <summary>
        /// Tam ung
        /// </summary>
        Advance=1,
        /// <summary>
        /// Thanh toán
        /// </summary>
        Payment = 2, 
        /// <summary>
        /// Khác
        /// </summary>
        Other=3
    }
    public class ContractPayment : BaseEntity
    {
        public ContractPayment()
        {
            this.PaymentGuid = Guid.NewGuid();
        }
        public Guid PaymentGuid { get; set; }
        public String Code { get; set; }
        public String Description { get; set; }
        public Int32? ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public Decimal Amount { get; set; }
        public Int32 CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        public Decimal CurrencyRatio { get; set; }
        public Int32 StatusId { get; set; }
        public ContractPaymentStatus paymentStatus
        {
            get => (ContractPaymentStatus)StatusId;
            set => StatusId = (int)value;
        }
        public Int32 TypeId { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
        public Int32? ApproverId { get; set; }
        public virtual Customer approver { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public String CodeRef { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Boolean? IsReceipts { get; set; }
        public Int32? UnitId { get; set; }
        public virtual Unit unitInfo { get; set; }
        public Int32? ProcuringAgencyId { get; set; }
        public virtual ProcuringAgency procuringAgency { get; set; }
        public Int32? PaymentRequestId { get; set; }
        public Int32? AcceptanceId { get; set; }
        public Int32? TaskId { get; set; }
        public virtual Task task { get; set; }
        public Int32? ContractIdBB { get; set; }
        public virtual Contract contractbb { get; set; }
        public int? ExpenditureId { get; set; }
        public virtual PaymentExpenditure paymentExpenditure { get; set; }

    }
}
