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
    [Validator(typeof(ContractPaymentPlanValidator))]
    public class ContractPaymentPlanModel : BaseGSEntityModel
    {
        public ContractPaymentPlanModel()
        {
            SelectedTaskIds = new List<int>();           
            Availbletasks = new List<SelectListItem>();
            lstPaymentPlanContract = new List<PaymentPlanContract>();
            paymentType = ContractPaymentType.Advance;
        }
        public Int32 ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.PayDate")]
        [UIHint("Date")]
        public DateTime? PayDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.Description")]
        public String Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.PayRule")]
        public String PayRule { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.Amount")]
        public Int64 Amount { get; set; }
        public string AmountText { get; set; }
        [UIHint("WorkCurrency")]
        public string AmountAdvance { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.PercentNum")]
        public decimal PercentNum { get; set; }
        
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentPlan.Fields.TypeId")]
        public int TypeId { get; set; }
        public ContractPaymentType paymentType
        {
            get => (ContractPaymentType)TypeId;
            set => TypeId = (int)value;
        }
        public string CurrencyName { get; set; }
        public int TaskId { get; set; }
        public Int64 totalAmountContract { get; set; }
        public string AmountSummary { get; set; }
        public string suggestionsPaymentPlan { get; set; }
        //addmore   
        public IList<int> SelectedTaskIds { get; set; }
        public List<SelectListItem> Availbletasks { get; set; }
        public int RequestCount { get; set; }
        public string ckPercentNumNote { get; set; }
        public int AppendixId { get; set; }
        public IList<PaymentPlanContract> lstPaymentPlanContract { get; set; }
        public Boolean IsFinishRequested { get; set; }
    }
    public class ContractPaymentPlanRule
    {
        public IList<int> lstTaskId { get; set; }
    }
    public partial class ContractPaymentPlanListModel : BasePagedListModel<ContractPaymentPlanModel>
    {

    }
    public partial class ContractPaymentPlanSearchModel : BaseSearchModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }

    }    
}
