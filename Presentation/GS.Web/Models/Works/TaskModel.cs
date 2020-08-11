using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Models.Contracts;
using GS.Web.Validators.Works;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Works
{
    [Validator(typeof(TaskValidator))]
    public class TaskModel : BaseGSEntityModel
    {
        public TaskModel()
        {
            AvailableTask = new List<SelectListItem>();
            AvailableCustomer = new List<SelectListItem>();
            AvailableTaskGroup = new List<SelectListItem>();
            AvailableCurrency = new List<SelectListItem>();
            AvailableUnit = new List<SelectListItem>();
            taskResourceModels = new List<TaskResourceModel>();
            taskContractModels = new List<TaskContractModel>();
            taskDeadLineModel = new List<TaskModel>();
            contractAcceptanceBB = new ContractAcceptanceBBModel();
            listPaymentBB = new List<ContractPaymentModel>();
            AvailebleContractType = new List<SelectListItem>();
            lstProcuringAgency = new List<SelectListItem>();
            //lsTaskLevelId = new SelectList;
        }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.ParentId")]
        public int ParentId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TreeNode")]
        public string TreeNode { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TreeLevel")]
        public int TreeLevel { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskGuid")]
        public Guid TaskGuid { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskCode")]
        public String Code { get; set; }
        public int CreatorId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [UIHint("Date")]
        [GSResourceDisplayName("AppWork.Works.Task.Fields.ContractId")]
        public int ContractId { get; set; }

        
        public int ApproverId { get; set; }

        public int AppendixId { get; set; }

        public int StatusId { get; set; }
        public TaskStatus taskStatus
        {
            get => (TaskStatus)StatusId;
            set => StatusId = (int)value;
        }
        public string taskStatusText { get; set; }
        public int PreTaskId { get; set; }
        /// <summary>
        /// ngay bat dau
        /// </summary>
        [GSResourceDisplayName("AppWork.Works.Task.Fields.StartDate")]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.DurationTime")]
        public int DurationTime { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.DurationUnit")]
        public int DurationUnit { get; set; }
        /// <summary>
        /// ngay du kien ket thuc
        /// </summary>
        [GSResourceDisplayName("AppWork.Works.Task.Fields.EndDate")]
        [UIHint("Date")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ngay ket thuc thuc te
        /// </summary>
        [GSResourceDisplayName("AppWork.Works.Task.Fields.FinishedDate")]
        [UIHint("Date")]
        public DateTime? FinishedDate { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.ResponsibleId")]
        public int? ResponsibleId { get; set; }
        public int StoreId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.UnitId")]       
        public int UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.PercentNum")]
        public Decimal? PercentNum { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.MeasureId")]
        public int MeasureId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.MeasureMass")]
        public Decimal? MeasureMass { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.MeasurePrice")]
        public Decimal? MeasurePrice { get; set; }
        [GSResourceDisplayName(" AppWork.Works.Task.Fields.TotalMoney")]
        [UIHint("WorkCurrency")]
        //gia tri sau thue
        public string TotalMoney { get; set; }
        [GSResourceDisplayName(" AppWork.Works.Task.Fields.PreTaxTotalMoney")]
        [UIHint("WorkCurrency")]
        //gia tri truoc thue
        public string PreTaxTotalMoney { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.note")]
        public String Note { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskLevelId")]
        public int TaskLevelId { get; set; }
        public SelectList lsTaskLevelId { get; set; }
        public int TypeId { get; set; }
        public Boolean IsTemplate { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.CurrencyId")]
        public int CurrencyId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskGroupId")]
        public int TaskGroupId { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskGroupValue")]
        public decimal TaskGroupValue { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskGroupSalary")]
        public decimal TaskGroupSalary { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaskGroupCost")]
        public decimal TaskGroupCost { get; set; }
        public int? CountChild { get; set; }
        public int? CountContractAcceptance { get; set; }
        [GSResourceDisplayName("AppWork.Works.Task.Fields.ContractTypeId")]
        public int ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        /// <summary>
        /// pham tran thue
        /// </summary>
        [GSResourceDisplayName("AppWork.Works.Task.Fields.TaxPercent")]
        public decimal? TaxPercent { get; set; }
        public int TaskProcuringAgencyId { get; set; }
        //add more 
        public string TaskProcuringAgencyName { get; set; }
        /// <summary>
        /// gia tri do dang
        /// </summary>
        public decimal Unfinished { get; set; }
        public string UnfinishedText { get; set; }
        /// <summary>
        /// Tong gia tri tam ung san luong
        /// </summary>
        public String AdvanceQuantityValue { get; set; }
        public Int64 PaymentSubValue { get; set; }
        public String classStatus { get; set; }
        public String nameStatus { get; set; }
        public String countDown { get; set; }
        public ContractAcceptanceListModel listContractAcceptanceTask { get; set; }
        /// <summary>
        /// danh sach hop dong phu luc
        /// </summary>
        public ContractListModel listAppendixIdContract { get; set; }
        /// <summary>
        /// danh sach hop dong BB'
        /// </summary>
        public ContractListModel listBBContract { get; set; }       
        public string ContractTaskName { get; set; }
        public Guid ContractTaskGuid { get; set; }
        public string TotalAmountText { get; set; }
        public string TaskGroupName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        //List Task
        /// <summary>
        /// ten don vi
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// gioi han tam ung cho don vi
        /// </summary>
        public string TotalLimitAdvance { get; set; }
        /// <summary>
        /// da tam ung cua don vi
        /// </summary>
        public string TotalAdvanced { get; set; }
        /// <summary>
        /// tong nghiem thu noi bo
        /// </summary>
        public string TotalAdvancedAcceptanceNoiBo { get; set; }

        public IList<SelectListItem> AvailebleContractType { get; set; }
        public IList<SelectListItem> AvailableTask { get; set; }
        public IList<SelectListItem> AvailableCustomer { get; set; }
        public IList<SelectListItem> AvailableTaskGroup { get; set; }
        public IList<SelectListItem> AvailableCurrency { get; set; }
        public IList<SelectListItem> AvailableUnit { get; set; }
        public IList<TaskResourceModel> taskResourceModels { get; set; }
        public IList<TaskContractModel> taskContractModels { get; set; }
        public IList<SelectListItem> lstProcuringAgency { get; set; }
        public IList<TaskModel> taskDeadLineModel { get; set; }
        /// <summary>
        /// danh sach tam ung san luong
        /// </summary>
        public ContractAcceptanceListModel listAdvanceQuantity { get; set; }
        /// <summary>
        /// nghiem thu BB'
        /// </summary>
        public ContractAcceptanceBBModel contractAcceptanceBB { get; set; }
        /// <summary>
        /// list thanh toan BB
        /// </summary>
        public IList<ContractPaymentModel> listPaymentBB { get; set; }
        /// <summary>
        /// gia tri thanh toan BB
        /// </summary>
        public string TotalAmountBBText { get; set; }
        /// <summary>
        /// gia tri da nghiem thu khoi luong
        /// </summary>
        public string TotalAmountAcceptanced { get; set;}
        /// <summary>
        /// gia tri da nghiem thu thanh toan
        /// </summary>
        public string TotalAmountPaymentAcceptanced { get; set; }
        /// <summary>
        /// gia tri da quyet toan
        /// </summary>
        public string TaskSettlemented { get; set; }
        public bool isAcceptanceKL { get; set; }
        [UIHint("Date")]
        public DateTime ContractEndDate { get; set; }
        [UIHint("Date")]
        public DateTime ContractStarDate { get; set; }
    }
    public partial class TaskListModel : BasePagedListModel<TaskModel>
    {

    }
    public partial class TaskSearchModel : BaseSearchModel
    {
        public int ContractId { get; set; }
        public int ParentId { get; set; }
    }
    public partial class TaskNoteModel
    {
        public TaskNoteModel()
        {
            
        }        
        public int Code { get; set; }
        public string Message { get; set; }
        public string IdRecord { get; set; }
        public TaskModel ObjectInfo { get; set; }
    }
}
