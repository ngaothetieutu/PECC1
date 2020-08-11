using FluentValidation.Attributes;
using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Models.Contracts;
using GS.Web.Validators.PaymentAdvance;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.PaymentAdvance
{
    [Validator(typeof(ContractPaymentAdvanceValidator))]
    public class ContractPaymentAdvanceModel : BaseGSEntityModel
    {
        public ContractPaymentAdvanceModel()
        {
            AvailableUnit = new List<SelectListItem>();
            AvailableContract = new List<SelectListItem>();
            AvailableTask = new List<SelectListItem>();
            ListContractAcceptance = new List<ContractAcceptanceModel>();
            listContractModel = new List<ContractModel>();
            listCurrency = new List<SelectListItem>();
            listSelectedContractIds = new List<int>(); 
        }
        public Guid AdvanceGuid { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.Name")]
        public string Name { get; set; }
        public int TypeId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.UnitId")]
        public int UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.PayDate")]
        [UIHint("DateNullable")]
        public DateTime? PayDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.CurrencyId")]
        public Int32? CurrencyId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatorId { get; set; }
        [UIHint("WorkCurrency")]
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.TotalAmount")]
        public string TotalAmount { get; set; }
        [UIHint("WorkCurrency")]
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAdvanceModel.Fields.TotalReceive")]
        public string TotalReceive { get; set; }
        public virtual string UnitName { get; set; }
        public List<SelectListItem> AvailableUnit { get; set; }
        public List<SelectListItem> AvailableContract { get; set; }
        public List<SelectListItem> AvailableTask { get; set; }
        public List<ContractAcceptanceModel> ListContractAcceptance { get; set; }
        public List<ContractModel> listContractModel { get; set; }
        public List<SelectListItem> listCurrency { get; set; }
        public IList<int> listSelectedContractIds { get; set; }
        public string ListContractId { get; set; }
        public string TotalAmountText { get; set; }
        public string TotalReceiveText { get; set; }
    }
    public class ContractPaymentAdvanceSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.PaymentAdvanceSearch.Fields.UnitId")]
        public Int32? UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentAdvanceSearch.Fields.KeySearch")]
        public string Keysearch { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentAdvanceSearch.Fields.FromDate")]
        [UIHint("DateNullable")]
        public DateTime? FromDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentAdvanceSearch.Fields.ToDate")]
        [UIHint("DateNullable")]
        public DateTime? ToDate { get; set; }
        public List<SelectListItem> AvailableUnit { get; set; }
    }
    public class ContractPaymentAdvanceListModel : BasePagedListModel<ContractPaymentAdvanceModel>
    {

    }
}
