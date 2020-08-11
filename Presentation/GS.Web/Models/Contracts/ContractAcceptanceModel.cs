using FluentValidation.Attributes;
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
    [Validator(typeof(ContractAcceptanceValidator))]
    public class ContractAcceptanceModel : BaseGSEntityModel
    {
        public ContractAcceptanceModel()
        {
            AvailableTaskList = new List<SelectListItem>();
            AvailableUnit = new List<SelectListItem>();
            SelectedListTaskId = new List<int>();
            SelectedListFileId = new List<int>();
            AvailableTaskGroup = new List<SelectListItem>();
            AvailableContract = new List<SelectListItem>();
            contractAcceptanceStatus = ContractAcceptanceStatus.Draf;
            listAcceptanceSub = new List<ContractAcceptanceSubModel>();
            ListTaskAvailable = new List<TaskModel>();
            listContractModel = new List<ContractModel>();
            listpaymentAcceptanceSub = new List<ContractPaymentAcceptanceSubModel>();
        }
        public Guid AcceptanceGuid { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.Code")]
        public String Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.Name")]
        public String Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.Description")]
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.ContractId")]
        public Int32 ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.ApprovalId")]
        public Int32? ApprovalId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.ApprovalDate")]
        [UIHint("DateNullable")]
        public DateTime? ApprovalDate { get; set; }
        public string DateApproval { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.Ratio")]
        public Decimal? Ratio { get; set; }
        public Int32 StatusId { get; set; }
        public ContractAcceptanceStatus contractAcceptanceStatus
        {
            get => (ContractAcceptanceStatus)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.TotalAmount")]
        [UIHint("WorkCurrency")]
        public string TotalAmount { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.ResponsibleId")]
        public Int32? ResponsibleId { get; set; }
        public Int32 TypeId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.CurrencyId")]
        public Int32? CurrencyId { get; set; }
        public String DataInfo { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.File")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.UnitId")]
        public Int32? UnitId { get; set; }
        public Int32? PaymentAdvanceId { get; set; }
        //add more
        public string UnitName { get; set; }
        public string ListContractId { get; set; }
        public List<ContractModel> listContractModel { get; set; }
        /// <summary>
        /// tien cua cong viec dang Int
        /// </summary>
        public Decimal? TaskTotalMoney { get; set; }
        /// <summary>
        /// so tien cua cong viec dang text
        /// </summary>  
        public string TaskTotalMoneyText { get; set; }
        /// <summary>
        /// ten cong viec
        /// </summary>
        public string TaskName { get; set; }
        public decimal? TaskGroupValue { get; set; }
        [GSResourceDisplayName(" AppWork.Contracts.ContractAcceptance.Fields.TaskGroupId")]
        public int TaskGroupId { get; set; }
        public string TotalAmountText { get; set; }
        public string CurrencyName { get; set; }
        public Int32? TaskId { get; set; }
        public Int32? PaymentAcceptanceId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.AvailableTaskList")]
        public List<SelectListItem> AvailableTaskList { get; set; }
        public IList<int> SelectedListTaskId { get; set; }
        public IList<int> SelectedListFileId { get; set; }
        public IList<SelectListItem> AvailableTaskGroup { get; set; }
        public IList<ContractAcceptanceSubModel> listAcceptanceSub { get; set; }
        public IList<ContractPaymentAcceptanceSubModel> listpaymentAcceptanceSub { get; set; }
        public IList<SelectListItem> AvailableContract { get; set; }
        public IList<TaskModel> ListTaskAvailable { get; set; }
        public IList<SelectListItem> AvailableUnit { get; set; }
        public string AmountPaymentAcceptance { get; set; }
        /// <summary>
        /// tong tien theo cong viec
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.SumToltalAmount")]
        public Int64 SumToltalAmount { get; set; }
        /// <summary>
        /// Tong so da ung dang Int
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.ToltalAmountAdvanced")]
        public Int64 ToltalAmountAdvanced{ get; set; }
        public string SumToltalAmountText { get; set; }
        /// <summary>
        /// tong so tien da ung dang text
        /// </summary>
        public string ToltalAmountAdvancedText { get; set; }
        /// <summary>
        /// tong tien da nhan
        /// </summary>
        public string TotalAmountReceived { get; set; }
        /// <summary>
        /// tong tien co the nhan toi da
        /// </summary>
        public string TotalAmountMaxReceived { get; set; }
        public bool hasPaymentAcceptance { get; set; }
        ///<sumary>
        ///Tong tien cua nghiem thu thanh toan
        ///</sumary>
        public Decimal TotalAmountAcceptance { get; set; }
    }
    public partial class ContractAcceptanceListModel : BasePagedListModel<ContractAcceptanceModel>
    {

    }
    public partial class ContractAcceptanceSearchModel : BaseSearchModel
    {
        public int ContractId { get; set; }
        public int TaskId { get; set; }
        public int TypeId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.Keysearch")]
        public string Keysearch { get; set; }
        public IList<SelectListItem> AvailableUnit {get;set;}
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptance.Fields.UnitId")]
        public int UnitId { get; set; }
    }
    
}
