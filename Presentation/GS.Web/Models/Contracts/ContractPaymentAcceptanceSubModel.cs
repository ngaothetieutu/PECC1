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

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ContractPaymentAcceptanceSubValidator))]
    public class ContractPaymentAcceptanceSubModel : BaseGSEntityModel
    {
        public ContractPaymentAcceptanceSubModel()
        {
            AvailableUnits = new List<SelectListItem>();
            AvailableTasks = new List<SelectListItem>();
        }
        public Int32 AcceptanceId { get; set; }
        public Int32 PaymentAcceptanceId { get; set; }
        public Int32 ContractId { get; set; }
        public Int32? UnitId { get; set; }        
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptanceSub.Fields.Description")]
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptanceSub.Fields.Ratio")]
        public Decimal? Ratio { get; set; }
        public int StatusId { get; set; }
        public PaymentAcceptanceSubStatus contractPaymentAcceptanceSubStatus
        {
            get => (PaymentAcceptanceSubStatus)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptanceSub.Fields.TotalAmount")]
        public Int64? TotalAmount { get; set; }
        [UIHint("WorkCurrency")]
        public string TotalMoney { get; set; }
        public Int32? CurrencyId { get; set; }
        public string TotalAmountText { get; set; }
        public string CurrencyName { get; set; }
        public Int32? TaskId { get; set; }
        [UIHint("WorkCurrency")]
        public string ReduceAdvance { get; set; }
        [UIHint("WorkCurrency")]
        public string ReduceKeep { get; set; }
        [UIHint("WorkCurrency")]
        public string ReduceOther { get; set; }
        [UIHint("WorkCurrency")]
        public string Depreciation { get; set; }
        //add more 
        public string TotalmoneyText { get; set; }
        public string TaskName { get; set; }
        public string UnitName { get; set; }
        public IList<SelectListItem> AvailableUnits { get; set; }
        public IList<SelectListItem> AvailableTasks { get; set; }
        public string TotalPaymented { get; set; }
        public string SumTotalMoney { get; set; }
    }
    public partial class ContractPaymentAcceptanceSubListModel : BasePagedListModel<ContractPaymentAcceptanceSubModel>
    {
        
    }
    public partial class ContractPaymentAcceptanceSubSearchModel : BaseSearchModel
    {
        public int ContractId { get; set; }
        public int AcceptanceId { get; set; }
    }
}
