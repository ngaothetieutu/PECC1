using GS.Core.Domain.Advance;
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
    public enum ContractAcceptanceStatus
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
    public enum ContractAcceptancesType
    {
        /// <summary>
        /// nghiệm thu Khối lương
        /// </summary>
        KhoiLuong =1,
        /// <summary>
        /// nghiệm thu nội bộ
        /// </summary>
        NoiBo =2,
        /// <summary>
        /// nghiem thu BB'
        /// </summary>
        BB =3,       
        /// <summary>
        /// Tam ung san luong
        /// </summary>
        TamUng = 4,
    }
    public class ContractAcceptance : BaseEntity
    {
        public ContractAcceptance()
        {
            contractAcceptanceStatus = ContractAcceptanceStatus.Approved;           
            //this.TypeId = 1;
            AcceptanceGuid = Guid.NewGuid();
        }       
        public Guid AcceptanceGuid { get; set; }
        public String Code { get; set; }       
        public String Name { get; set; }
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public Int32? ApprovalId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public Decimal? Ratio { get; set; }
        public virtual Customer approver { get; set; }
        public Int32 StatusId { get; set; }
        public Decimal? TotalAmount { get; set; }
        public ContractAcceptanceStatus contractAcceptanceStatus
        {
            get => (ContractAcceptanceStatus)StatusId;
            set => StatusId = (int)value;
        }       
        public Int32? ResponsibleId { get; set; }
        public virtual Customer responsible { get; set; }   
        public Int32 TypeId { get; set; }       
        public Int32? CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        public String DataInfo { get; set; }
        public Int32? PaymentAcceptanceId { get; set; }
        public virtual ContractPaymentAcceptance contractPaymentAcceptance { get; set; }
        public Int32? UnitId { get; set; }
        public Int32? PaymentAdvanceId { get; set; }
        public virtual Unit unit { get; set; }
        public virtual ContractPaymentAdvance paymentAdvance { get; set; }
    }
}