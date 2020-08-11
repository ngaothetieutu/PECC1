using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public partial class ContractReport : BaseEntity
    {
        public int constructionId { get; set; }
        public int contractID { get; set; }
        public string ConstructionCode { get; set; }
        public string ConstructionName { get; set; }
        public string ProcuringAgencyName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ConstructionType { get; set; }
        public bool ProcuringAgencyIsEVN { get; set; }
        public Decimal? TotalAmount { get; set; }
        public String ContractForm { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime EndDate { get; set; }
        public Decimal AdvanceAmount { get; set; }
        public Decimal ProgressAcceptance { get; set; }
        public Decimal ProgressPaymentAcceptance { get; set; }
        public int StatusId { get; set; }
        public ContractStatus status
        {
            get => (ContractStatus)StatusId;
            set => StatusId = (int)value;
        }
        public DateTime? FinishDate { get; set; }
        public string CodeP4 { get; set; }
        public int constructionTypeId { get; set; }
    }
    public partial class ContractAcceptanceReport : BaseEntity
    {
        public ContractAcceptanceReport()
        {
            ConstructionName = "";
            Code = "";
            contractPeriod = "";
        }
        public int constructionId { get; set; }
        public string ConstructionCode { get; set; }
        public string ConstructionName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string contractPeriod { get; set; }
        //tổng nghiệm thu
        public Decimal? ContractPaymentAcceptance { get; set; }
        //trừ sản lượng tạm ứng
        public Decimal? ReduceAdvance { get; set; }
        //trừ khấu hao
        public Decimal? Depreciation { get; set; }
        //khoản giữ lại
        public Decimal? ReduceKeep { get; set; }
        //giảm trừ khác
        public Decimal? ReduceOrther { get; set; }
        // sản lượng còn lại
        public Decimal? TotalAmount { get; set; }
        //tỉ lệ khoán
        public Decimal? Ratio { get; set; }
        public string dateTime { get; set; }
        public string unitName { get; set; }
    }
    public partial class ContractReportBB
    {
        public int constructionId { get; set; }
        public String ConstructionCode { get; set; }
        public String ConstructionName { get; set; }
        public String ContractBBCode { get; set; }
        public String ContractName { get; set; }
        public String ContractPeriod { get; set; }
        public String ConstructionType { get; set; }
        public bool IsEVN { get; set; }
        public Decimal? TotalAmount { get; set; }
        public String ContractABCode { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime? Progress { get; set; }
        public String ClientCode { get; set; }
        public String ClientName { get; set; }
        public String UnitName { get; set; }
        public int ContractStatus { get; set; }
        public ContractStatus contractStatus
        {
            get => (ContractStatus)ContractStatus;
            set => ContractStatus = (int)value;
        }
    }
    public partial class ContractReportAB
    {
        public int constructionId { get; set; }
        public String ConstructionCode { get; set; }
        public String ConstructionName { get; set; }
        public String ContractCode { get; set; }
        public String ContractName { get; set; }
        public String ContractPeriod { get; set; }
        public String ConstructionType { get; set; }
        public bool IsEVN { get; set; }
        public Decimal? TotalAmount { get; set; }
        public String ContractForms { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime Progress { get; set; }
        public Decimal PlanAdvance { get; set; }
        public Decimal PlanPayment { get; set; }
        public String ClientCode { get; set; }
        public String ClientName { get; set; }
        public String UnitCode { get; set; }
        public Decimal TotalAdvance { get; set; }
        public String BillDate { get; set; }
        public int ContractStatus { get; set; }
        public ContractStatus contractStatus
        {
            get => (ContractStatus)ContractStatus;
            set => ContractStatus = (int)value;
        }
    }
    public partial class ReportProcuringAgency
    {
        public int ProcuringAgencyId { get; set; }
        public String ProcuringAgencyName { get; set; }
        public String ConstructionCode { get; set; }
        public String ConstructionName { get; set; }
        public String ContractCode { get; set; }
        public bool IsEVN { get; set; }
        public String ContractName { get; set; }
        public String CodeP4 { get; set; }
        public DateTime SignedDate { get; set; }
        public int ContractStatus { get; set; }
        public ContractStatus contractStatus
        {
            get => (ContractStatus)ContractStatus;
            set => ContractStatus = (int)value;
        }
        public Decimal? TotalAmount { get; set; }
        public Decimal AmountRequest { get; set; }
        public Decimal AmountPayment { get; set; }
    }
    public partial class ReportContractNotsigned
    {
        public int ConstructionId { get; set; }
        public String ConstructionName { get; set; }
        public String ConstructionCode { get; set; }
        public String ContractName { get; set; }
        public String ContractCode { get; set; }
        public String ProcuringAgency { get; set; }
        public bool isEvn { get; set; }
        public Decimal TotalAmount { get; set; }
        public int contractStatus { get; set; }
        public ContractStatus ContractStatus
        {
            get => (ContractStatus)contractStatus;
            set => contractStatus = (int)value;
        }
        public String ConstructionType { get; set; }
        public String ContractForm { get; set; }
    }
    public partial class ReportContractDealine
    {
        public int ConstructionId { get; set; }
        public String ConstructionName { get; set; }
        public String ConstructionCode { get; set; }
        public String ContractName { get; set; }
        public String ContractCode { get; set; }
        public String ProcuringAgency { get; set; }
        public bool isEvn { get; set; }
        public Decimal TotalAmount { get; set; }
        public String ConstructionType { get; set; }
        public String ContractForm { get; set; }
        public DateTime FinishDate { get; set; }
    }
    public partial class ReportTaskByUnit
    {
        public int UnitId { get; set; }
        public int ConstructionId { get; set; }
        public String ConstructionName { get; set; }
        public String ConstructionCode { get; set; }
        public String ContractName { get; set; }
        public String ContractCode { get; set; }
        public String UnitName { get; set; }
        public String ProcuringAgencyName { get; set; }
        public bool isEvn { get; set; }
        public Decimal ContractTotalAmount { get; set; }
        public String ConstructionType { get; set; }
        public String ContractForm { get; set; }
        public Decimal TaskTotalAmount { get; set; }
        /// <summary>
        /// Gia tri tam ung
        /// </summary>
        public Decimal PaymentAdvance { get; set; }
        /// <summary>
        /// Gia tri da NT
        /// </summary>
        public Decimal PaymentAcceptance { get; set; }
        public int tasktStatus { get; set; }
        public TaskStatus TaskStatus
        {
            get => (TaskStatus)tasktStatus;
            set => tasktStatus = (int)value;
        }
        public string TaskName { get; set; }
        public DateTime? time { get; set; }
        public Decimal ContractUnfinish1 { get; set; }
        public Decimal ContractUnfinish2 { get; set; }
        public Decimal ContractNoWork { get; set; }
    }
    public partial class ReportContractUnfinish{
        public int ConstructionId { get; set; }
        public String ConstructionName { get; set; }
        public String ConstructionCode { get; set; }
        public String ContractName { get; set; }
        public String ContractCode { get; set; }
        public DateTime SignedDate { get; set; }
        public decimal TotalAmount { get; set; }
        /// gia tri NTTT
        public decimal PaymentAmount { get; set; }
        /// thiet ke
        public Decimal ContractFormDesign { get; set; }
        /// khao sat
        public Decimal ContractFormTested { get; set; }
        /// unfinish2 contract
        public Decimal ContractUnfinish2 { get; set; }
        public Boolean isSettlemented { get; set; }
        public int UserMonitorId { get; set; }
    }
}
