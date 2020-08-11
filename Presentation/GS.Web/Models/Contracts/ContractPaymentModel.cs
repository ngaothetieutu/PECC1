using FluentValidation.Attributes;
using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ContractPaymentValidator))]
    public class ContractPaymentModel : BaseGSEntityModel
    {
        public ContractPaymentModel()
        {
            AvailableTasks = new List<SelectListItem>();
            AvailableCurrency = new List<SelectListItem>();
            selectTypeId = new List<SelectListItem>();
            //ContractPayments = new List<ContractPaymentModel>();
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.PaymentGuid")]
        public Guid PaymentGuid { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ContractId")]
        public int ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ContractId")]
        public int CreatorId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.Amount")]
        [UIHint("WorkCurrency")]
        public string Amount { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.CurrencyId")]
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.CurrencyRatio")]
        public decimal CurrencyRatio { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.StatusId")]
        public int StatusId { get; set; }

        public ContractPaymentStatus paymentStatus
        {
            get => (ContractPaymentStatus)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.TypeId")]
        public int TypeId { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
        public string TypeText { get; set; }
        public SelectList lstType { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ApproverId")]
        public int ApproverId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.UpdatedDate")]
        public DateTime UpdatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ApprovedDate")]
        [UIHint("Date")]
        public DateTime ApprovedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.CodeRef")]
        public string CodeRef { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.PaymentDate")]
        [UIHint("Date")]
        public DateTime PaymentDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.IsReceipts")]
        public bool IsReceipts { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.UnitId")]
        public int UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ProcuringAgencyId")]
        public int ProcuringAgencyId { get; set; }
        public IList<SelectListItem> AvailableCurrency { get; set; }
        public int? PaymentRequestId { get; set; }
        public int? AcceptanceId { get; set; }
        public int PaymentPlanId { get; set; }
        public int TaskId { get; set; }
        public string TotalAmountText { get; set; }
        public IList<SelectListItem> AvailableTasks { get; set; }
        public int ExpenditureId { get; set; }
       
        //add more 
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.TaskName")]
        public string TaskName { get; set; }
        /// <summary>
        /// Đã nghiệm thu        
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ContractAcceptanced")]
        public string ContractAcceptanced { get; set; }
        /// <summary>
        /// đã thanh toán
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.ContractPaymented")]
        public string ContractPaymented { get; set; }
        public IList<SelectListItem> selectTypeId { get; set;} 
        public string ContractName { get; set; }
    }
    public partial class ContractPaymentListModel : BasePagedListModel<ContractPaymentModel>
    {

    }
    public partial class ContractPaymentSearchModel : BaseSearchModel
    {
        public ContractPaymentSearchModel()
        {
            AvailableTasks = new List<SelectListItem>();
            SelectListTaskId = new List<int>();
        }        
        public int ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Search.Fields.Description")]
        public string Keysearch { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.IsReceipts")]
        public int IsReceipts { get; set; }
        public SelectList lsIsReceipts { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPayment.Fields.TypeId")]
        public int TypeId { get; set; }
        public SelectList lsType { get; set; }         
        public IList<int> SelectListTaskId { get; set; }
        public List<SelectListItem> AvailableTasks { get; set; }
    }
   
}
