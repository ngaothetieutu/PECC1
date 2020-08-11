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
    [Validator(typeof(ContractPaymentRequestValidator))]
    public class ContractPaymentRequestModel : BaseGSEntityModel
    {
        public ContractPaymentRequestModel()
        {
            contractpaymentRequestModels = new List<ContractPaymentRequestModel>();
            SelectedContractPaymentAcceptence = new List<int>();
            AvailableContractPaymentAcceptence = new List<SelectListItem>();
            paymentType = ContractPaymentType.Advance;
        }

        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.TypeId")]
        public int TypeId { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.ContractId")]
        public int ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.PaymentPlanId")]
        public int PaymentPlanId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.CreatorId")]
        public int CreatorId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.TotalAmount")]
        [UIHint("WorkCurrency")]
        public string TotalAmount { get; set; }
        public string TotalAmountText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.DataInfo")]
        public string DataInfo { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.RequestDate")]
        [UIHint("Date")]
        public DateTime RequestDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.EndDate")]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.TaxRatio")]
        public decimal TaxRatio { get; set; }
        public List<ContractPaymentRequestModel> contractpaymentRequestModels { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.PaymentAcceptenceId")]
        public int PaymentAcceptenceId { get; set; }
        public IList<int> SelectedContractPaymentAcceptence { get; set; }
        public IList<SelectListItem> AvailableContractPaymentAcceptence { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentRequest.Fields.AmountPayment")]
        public string AmountPayment { get; set; }
        public Boolean IsPaymented { get; set; }
    }
    public partial class ContractPaymentRequestListModel : BasePagedListModel<ContractPaymentRequestModel>
    {
        public int ContractId { get; set; }
        public int PaymentPlanId { get; set; }
        public int TypeId { get; set; }
    }
    public partial class ContractPaymentRequestSearchModel : BaseSearchModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }

    }
}
