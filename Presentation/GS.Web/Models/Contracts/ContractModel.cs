using FluentValidation.Attributes;
using GS.Core;
using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Models.Works;
using GS.Web.Validators.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ContractValidator))]
    public class ContractModel : BaseGSEntityModel
    {
        public ContractModel()
        {
            SelectedContractFormIds = new List<int>();
            AvailableContractForm = new List<SelectListItem>();
            SelectedContractTypeIds = new List<int>();
            AvailableContractType = new List<SelectListItem>();
            SelectedContractPeriodIds = new List<int>();
            SelectedPeriodIds = new List<int>();
            AvailableContractPeriod = new List<SelectListItem>();
            contractlogModels = new List<ContractLogModel>();
            contractFormModels = new List<ContractFormModel>();
            contractTypeModels = new List<ContractTypeModel>();
            contractPeriodModels = new List<ContractPeriodModel>();
            LstContractPayments = new List<ContractPaymentModel>();
            contractResourceModels = new List<ContractResourceModel>();
            contractRelateModels = new List<ContractModel>();
            contractAppendixModels = new List<ContractModel>();
            lstContractPaymentPlan = new List<ContractPaymentPlanModel>();
            lstContractPaymentAcceptance = new List<ContractPaymentAcceptanceModel>();
            contractpaymentRequestModels = new List<ContractPaymentRequestModel>();
            lstContractAcceptance = new List<ContractAcceptanceModel>();
            lstContractAcceptanceNoiBo = new List<ContractAcceptanceModel>();
            lstTask = new List<TaskModel>();
            AvailableTask = new List<SelectListItem>();
            DisplayType = ContractDisplayType.Row;
            contractMonitor = new ContractMonitorModel();
            BarCharts barCharts = new BarCharts();
            lsContractSettlement = new List<ContractSettlementModel>();
            LstTaskModel = new List<TaskModel>();
            LstTaskModelChuaThucHien = new List<TaskModel>();
            LstTaskModelDangThucHien = new List<TaskModel>();
            lstContractAcceptanceSub = new List<ContractAcceptanceSubModel>();
            lstContractUnfinishModel = new List<ContractUnfinishModel>();
            lstContractUnfinish2Model = new List<ContractUnfinishModel>();
            lstContractUnfinishModel2 = new List<ContractUnfinishModel>();
        }
        /// <summary>
        /// Hien thi thong tin hop dong theo dang box or row
        /// </summary>
        public ContractDisplayType DisplayType { get; set; }

        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.ContractGuid")]
        public Guid ContractGuid { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.Code1")]
        public string Code1 { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.ClassificationId")]
        public int ClassificationId { get; set; }
        public ContractClassification classification
        {
            get => (ContractClassification)ClassificationId;
            set => ClassificationId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.ClassificationText")]
        public string classificationText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.PeriodId")]
        public int PeriodId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.CreatorId")]
        public int CreatorId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.ApproverId")]
        public int ApproverId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.SearchFullText")]
        public string SearchFullText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.StartDate")]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.EndDate")]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }
        // Cảnh báo thời gian
        //public int CompareDate { get; set; }
        //public int interval { get; set; }

        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.StatusId")]
        public int StatusId { get; set; }
        public ContractStatus status
        {
            get => (ContractStatus)StatusId;
            set => StatusId = (int)value;
        }
        public string statusText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.ConstructionId")]
        public int ConstructionId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.SignedDate")]
        [UIHint("Date")]
        public DateTime SignedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.Note")]
        public string Note { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.StoreId")]
        public int StoreId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.IsTemplate")]
        public bool IsTemplate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.PaymentInfo")]
        public string PaymentInfo { get; set; }
        [UIHint("DateNullable")]
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.FinishedDate")]
        public DateTime? FinishedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.TotalAmount")]
        public Int64 TotalAmount { get; set; }
        [UIHint("WorkCurrency")]
        public string TotalAmountNumber { get; set; }
        public String TotalAmountText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.CurrencyId")]
        public int CurrencyId { get; set; }
        public string ConstructionName { get; set; }
        public string ContractPeriodName { get; set; }
        public string StoreName { get; set; }
        public string CurrencyName { get; set; }
        public string AmountSummary { get; set; }
        /// <summary>
        /// GT dang do chua nghiem thu
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.unfinished1")]
        [UIHint("WorkCurrency")]
        public string Unfinished1 { get; set; }
        /// <summary>
        /// GT dang do chua thuc hien
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.unfinished2")]
        [UIHint("WorkCurrency")]
        public string Unfinished2 { get; set; }
        /// <summary>
        /// GT dang do hien tai
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.unfinished2")]
        [UIHint("WorkCurrency")]
        public string Unfinished3 { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.TaxPercent")]
        public Decimal TaxPercent { get; set; }
        public int ContractRelateId { get; set; }
        public ProcuringAgencyModel procuringAgencyModelSideA { get; set; }
        //add more         
        public bool AppendixAB { get; set; }
        public string totalAmountBB { get; set; }
        // Tong gia tri cong viec lien danh cua hop dong
        public string totalAmountContact { get; set; }
        public string AllTotalAmountText { get; set; }
        public int ProgressNum
        {
            get
            {
                return CommonHelper.ProgressNumByDatetime(StartDate, EndDate);
            }
        }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.TaskIdBB")]
        public int TaskIdBB { get; set; }
        public List<SelectListItem> AvailableTask { get; set; }
        public List<ContractPaymentPlanModel> lstContractPaymentPlan { get; set; }
        public List<ProcuringAgencyModel> procuringAgencyModelSideB { get; set; }
        public List<ContractJointModel> contractJoinModels { get; set; }
        public List<ContractLogModel> contractlogModels { get; set; }
        public List<ContractPaymentRequestModel> contractpaymentRequestModels { get; set; }
        public IList<SelectListItem> AvailableContruction { get; set; }
        public List<TaskModel> lstTask { get; set; }

        public IList<SelectListItem> AvailableStore { get; set; }
        public IList<SelectListItem> AvailableCurrency { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.SelectedContractFormIds")]
        public IList<int> SelectedContractFormIds { get; set; }
        public List<ContractFormModel> contractFormModels { get; set; }
        public IList<SelectListItem> AvailableContractForm { get; set; }
        public List<ContractModel> contractRelateModels { get; set; }
        public string classificationTextContractRelate { get; set; }

        public List<ContractModel> contractAppendixModels { get; set; }
        public string classificationTextContractAppendix { get; set; }
        public int AppendixId { get; set; }


        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.SelectedContractTypeIds")]
        public IList<int> SelectedContractTypeIds { get; set; }
        public List<ContractTypeModel> contractTypeModels { get; set; }
        public IList<SelectListItem> AvailableContractType { get; set; }

        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.SelectedContractPeriodIds")]
        public IList<int> SelectedContractPeriodIds { get; set; }
        public IList<int> SelectedPeriodIds { get; set; }
        public List<ContractPeriodModel> contractPeriodModels { get; set; }
        public IList<SelectListItem> AvailableContractPeriod { get; set; }
        public ContractModel ContractAB { get; set; }
        public List<ContractPaymentModel> LstContractPayments { get; set; }
        public ContractPaymentModel contractPaymentModel { get; set; }
        public List<WorkFileModel> workFileModels { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Contract.Fields.WorkFiles")]
        [UIHint("WorkFile")]
        public string workFileIds { get; set; }
        public List<ContractResourceModel> contractResourceModels { get; set; }
        public List<ContractPaymentAcceptanceModel> lstContractPaymentAcceptance { get; set; }
        public List<ContractAcceptanceModel> lstContractAcceptance { get; set; }
        public List<ContractAcceptanceSubModel> lstContractAcceptanceSub { get; set; }
        public List<ContractAcceptanceModel> lstContractAcceptanceNoiBo { get; set; }
        public List<ContractSettlementModel> lsContractSettlement { get; set; }
        public string suggestionCreatBB { get; set; }
        public ContractMonitorModel contractMonitor { get; set; }
        public Guid contractGuidAB { get; set; }
        public List<TaskModel> LstTaskModel { get; set; }
        public List<TaskModel> LstTaskModelChuaThucHien { get; set; }
        public List<TaskModel> LstTaskModelDangThucHien { get; set; }
        public string TotalContractAcceptanceKLText { get; set; }
        public string TotalPaymentAdvanceContractText { get; set; }
        public string TotalPaymentPaidContractText { get; set; }
        public IList<ContractUnfinishModel> lstContractUnfinishModel { get; set; }
        public IList<ContractUnfinishModel> lstContractUnfinish2Model { get; set; }
        public IList<ContractUnfinishModel> lstContractUnfinishModel2 { get; set; }
    }
    public class ContractMoneyPlan
    {
        public string totalhasPaymentPlan { get; set; }
        public string totalPaymentPlan { get; set; }
        public string totalAmountCollected { get; set; }
        public string totalAmountSpent { get; set; }
        public string totalAmountReceivable { get; set; }
        public string totalAmountSpend { get; set; }
        public string totalAmountContract { get; set; }
        public string totalAmountAccepted { get; set; }
        public string totalAmountNotAccepted { get; set; }
        public string totalAmountBB { get; set; }
        public string totalAcceptanced { get; set; }
        public string noteSpend { get; set; }
        public string noteReceivable { get; set; }
        public string notePaymentPlan { get; set; }
    }
    public partial class ContractListModel : BasePagedListModel<ContractModel>
    {
        public int CurrentPage { get; set; }
        public int CurrentPageTo { get; set; }
        public int CurrentPageFrom { get; set; }
        public int TotalPage { get; set; }
        public int TypeOrder { get; set; }
        public int TypeDisplay { get; set; }
    }
    public enum ContractDisplayType
    {
        /// <summary>
        /// Hien thi dang hop
        /// </summary>
        Box = 1,
        /// <summary>
        /// Hien thi dang hang
        /// </summary>
        Row = 2
    }
    public enum ContractListOrderType
    {
        /// <summary>
        /// ngay bat dau tang dan
        /// </summary>
        StartDateAsc = 0,
        /// <summary>
        /// ngay bat dau giam dan
        /// </summary>
        StartDateDes = 1,
        /// <summary>
        /// gia tri tang dan
        /// </summary>
        AmountAsc = 2,
        /// <summary>
        /// gia tri giam dan
        /// </summary>
        AmountDes = 3,
    }
    public partial class ContractSearchModel : BaseSearchModel
    {
        public ContractSearchModel()
        {
            DdlprocuringAgency = new List<SelectListItem>();
            DdlConstruction = new List<SelectListItem>();
            SelectedProcuringAgencyIds = new List<int>();
            SelectedConstructionIds = new List<int>();
            Display = ContractDisplayType.Row;
            PageSize = 12;
            DdlPageSizes = new List<SelectListItem>();
            DdlPageSizes.Add(new SelectListItem { Value = "12", Text = "12" });
            DdlPageSizes.Add(new SelectListItem { Value = "50", Text = "50" });
            DdlPageSizes.Add(new SelectListItem { Value = "100", Text = "100" });
        }
        public int ClassificationId { get; set; }
        public ContractClassification classification
        {
            get => (ContractClassification)ClassificationId;
            set => ClassificationId = (int)value;
        }
        public string Keysearch { get; set; }
        public string Id { get; set; }
        public int TypeOrder { get; set; }
        public int TypeDisplay { get; set; }
        public int ConstructionTypeId { get; set; }
        public List<SelectListItem> AvailableConstructionTypeId { get; set; }
        public IList<int> SelectedProcuringAgencyIds { get; set; }
        public IList<int> SelectedConstructionIds { get; set; }
        public ContractDisplayType Display
        {
            get => (ContractDisplayType)TypeDisplay;
            set => TypeDisplay = (int)value;
        }
        public ContractListOrderType Order
        {
            get => (ContractListOrderType)TypeOrder;
            set => TypeOrder = (int)value;
        }
        [UIHint("DateNullable")]
        public DateTime? StartDateFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? StartDateTo { get; set; }
        [UIHint("DateNullable")]
        public DateTime? SignedateFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? SignedateTo { get; set; }
        //public int ProcuringAgencyId { get; set; }
        public SelectList DdlClassification { get; set; }
        public SelectList DdlTypeOrder { get; set; }
        public SelectList DdlTypeDisplay { get; set; }
        public int contractForm { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public int contractMonitorStatus { get; set; }
        public ContractMonitorStatus ContractMonitorStatus
        {
            get => (ContractMonitorStatus)contractMonitorStatus;
            set => contractMonitorStatus = (int)value;
        }
        public List<SelectListItem> DdlprocuringAgency { get; set; }
        public List<SelectListItem> DdlConstruction { get; set; }
        public List<SelectListItem> DdlPageSizes { get; set; }
        public List<SelectListItem> AvaiableContractForms { get; set; }
        public SelectList AvaiableContractStatus { get; set; }
        public SelectList AvaiableContractMonitorStatus { get; set; }
        public int StartYear { get; set; }
        [UIHint("DateNullable")]
        public DateTime? AcceptanceDateFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? AcceptanceDateTo { get; set; }
    }

    public partial class ContractJointModel : BaseGSEntityModel
    {
        public Int32 ContractId { get; set; }
        public Int32 ProcuringAgencyId { get; set; }
        public string ProcuringAgencyData { get; set; }
        /// <summary>
        /// Thong tin chu dau tu tren hop dong
        /// </summary>
        public ProcuringAgencyModel procuringAgency { get; set; }
        public Boolean IsSideA { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Boolean IsMain { get; set; }
        public IList<SelectListItem> AvailableProcurignAgency { get; set; }
        public int taskId { get; set; }
    }
    public partial class ContractFileModel : BaseGSEntityModel
    {
        public Guid ContractGuid { get; set; }
        public string FileIds { get; set; }
        public string EntityId { get; set; }
        public int TypeId { get; set; }
        public ContractFileType contractFileType
        {
            get
            {
                return (ContractFileType)TypeId;
            }
            set
            {
                TypeId = (int)value;
            }
        }
    }
    public partial class ContractLiquidationModel
    {
        public ContractLiquidationModel()
        {
            SelectedListFileId = new List<int>();
        }
        public int ContractId { get; set; }
        [UIHint("DateNullable")]
        [GSResourceDisplayName("AppWork.Contracts.Liquidation.Fields.LiquidationDate")]
        public DateTime? LiquidationDate { get; set; }
        public List<int> SelectedListFileId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Liquidation.Fields.LiquidationFileIds")]
        [UIHint("WorkFile")]
        public string LiquidationFileIds { get; set; }
    }
    public partial class ContractAppendixModel
    {
        public ContractAppendixModel()
        {
            taskContractNote = new List<TaskNoteModel>();

        }
        public Guid ContractGuidAB { get; set; }
        public IList<TaskModel> taskNew { get; set; }
        public IList<TaskModel> taskOld { get; set; }
        public IList<TaskNoteModel> taskContractNote { get; set; }
        public ContractModel ContractAppendix { get; set; }

    }
    public partial class AppendixEditPaymenPlanModel
    {
        public AppendixEditPaymenPlanModel()
        {
            AppendixEditPaymentPlanNote = new List<PaymentPlanNoteModel>();
            contractPaymentPlanNews = new List<ContractPaymentPlanModel>();
            contractPaymentPlanOlds = new List<ContractPaymentPlanModel>();
        }
        public Guid ContractGuidAB { get; set; }
        public ContractModel ContractAppendix { get; set; }
        public IList<PaymentPlanNoteModel> AppendixEditPaymentPlanNote { get; set; }
        public IList<ContractPaymentPlanModel> contractPaymentPlanNews { get; set; }
        public IList<ContractPaymentPlanModel> contractPaymentPlanOlds { get; set; }
    }
}
