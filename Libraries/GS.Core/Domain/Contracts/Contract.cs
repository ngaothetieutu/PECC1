using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GS.Core.Domain.Stores;
using GS.Core.Domain.Works;

namespace GS.Core.Domain.Contracts
{
    public enum ContractClassification
    {
        All = 0,
        /// <summary>
        /// Hợp đồng A - B
        /// </summary>
        AB = 1,
        /// <summary>
        /// Hợp đồng B - B'
        /// </summary>
        BB = 2,
        /// <summary>
        /// Phụ lục
        /// </summary>
        Appendix = 3
    }
    /// <summary>
    /// 1: draf, 2: approved, 4: destroy
    /// </summary>
    public enum ContractStatus
    {
        /// <summary>
        /// Tat ca
        /// </summary>
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
        Destroy = 4,
        /// <summary>
        /// Đang thực hiện hợp đồng
        /// </summary>
        Building = 5,
        /// <summary>
        /// Đã thanh lý
        /// </summary>
        Liquidated = 6,
        /// <summary>
        /// Ket thuc 
        /// </summary>
        Completed = 7,
        /// <summary>
        /// Da quyet toan
        /// </summary>
        Settlemented = 8,
        /// <summary>
        /// Nghiem thu khoi luong
        /// </summary>
        Acceptance = 9
    }
    public partial class ContractCurrency : BaseEntity
    {
        public int ContractId { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public partial class Contract : BaseEntity
    {

        private ICollection<ContractContractTypeMapping> _contractContractTypeMappings;
        private IList<ContractType> _contractTypes;
        private ICollection<ContractContractFormMapping> _contractContractFormMappings;
        private IList<ContractForm> _contractForms;
        private ICollection<ContractContractPeriodMapping> _contractContractPeriodMappings;
        private IList<ContractPeriod> _contractPeriods;
        public Contract()
        {
            this.ContractGuid = Guid.NewGuid();
        }
        /// <summary>
        /// Guid của hợp đồng, sử dụng để edit, hay liên kết các hệ thống với nhau
        /// </summary>
        public Guid ContractGuid { get; set; }
        /// <summary>
        /// Số hợp đồng
        /// </summary>
        public String Code { get; set; }
        public String Code1 { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Int32 ClassificationId { get; set; }
        public ContractClassification classification
        {
            get => (ContractClassification)ClassificationId;
            set => ClassificationId = (int)value;
        }
        public Int32? PeriodId { get; set; }
        //public virtual ContractPeriod period { get; set; }

        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public Int32? ApproverId { get; set; }
        public virtual Customer approver { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String SearchFullText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32 StatusId { get; set; }
        public ContractStatus status
        {
            get => (ContractStatus)StatusId;
            set => StatusId = (int)value;
        }
        public Int32? ConstructionId { get; set; }
        public virtual Construction construction { get; set; }
        public DateTime? SignedDate { get; set; }
        public String Note { get; set; }
        public Int32 StoreId { get; set; }
        public virtual Store store { get; set; }
        public Boolean IsTemplate { get; set; }
        public String PaymentInfo { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Decimal? TotalAmount { get; set; }
        public Int32? CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        /// <summary>
        /// Tong hop gia tri hop dong
        /// Vd: 1.300 triệu đ, 300 nghìn USD
        /// </summary>
        public string AmountSummary { get; set; }
        public Decimal? Unfinished1 { get; set; }
        public Decimal? Unfinished2 { get; set; }
        public Decimal? TaxPercent { get; set; }
        public virtual IList<ContractType> ContractTypes
        {
            get => _contractTypes ?? (_contractTypes = ContractContractTypeMappings.Select(m => m.contractType).ToList());
        }

        public virtual ICollection<ContractContractTypeMapping> ContractContractTypeMappings
        {
            get => _contractContractTypeMappings ?? (_contractContractTypeMappings = new List<ContractContractTypeMapping>());
            protected set => _contractContractTypeMappings = value;
        }

        public virtual IList<ContractForm> ContractForms
        {
            get => _contractForms ?? (_contractForms = ContractContractFormMappings.Select(m => m.contractForm).ToList());
        }

        public virtual ICollection<ContractContractFormMapping> ContractContractFormMappings
        {
            get => _contractContractFormMappings ?? (_contractContractFormMappings = new List<ContractContractFormMapping>());
            protected set => _contractContractFormMappings = value;
        }
        public virtual IList<ContractPeriod> ContractPeriods
        {
            get => _contractPeriods ?? (_contractPeriods = ContractContractPeriodMappings.Select(m => m.contractPeriod).ToList());
        }

        public virtual ICollection<ContractContractPeriodMapping> ContractContractPeriodMappings
        {
            get => _contractContractPeriodMappings ?? (_contractContractPeriodMappings = new List<ContractContractPeriodMapping>());
            protected set => _contractContractPeriodMappings = value;
        }
    }
}
