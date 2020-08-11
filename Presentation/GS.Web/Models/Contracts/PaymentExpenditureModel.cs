using FluentValidation.Attributes;
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
    [Validator(typeof(PaymentExpenditureValidator))]
    public class PaymentExpenditureModel : BaseGSEntityModel
    {
        public PaymentExpenditureModel()
        {
            lstContractPayment = new List<ContractPaymentModel>();
            AvailableCurrency = new List<SelectListItem>();
        }
        public Guid ExpenditureGuid { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.Code")]
        public String Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.Name")]
        public String Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.Description")]
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TypeId { get; set; }
        public int UnitId { get; set; }
        public Decimal TotalAmount { get; set; }
        public int PaymentAdvanceId { get; set; }
        public int AcceptanceId { get; set; }
        //addmore
        public string TaskName { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.UnitName")]
        public string UnitName { get; set; }
        public string TotalAmounts { get; set; }
        public string CreatedDates { get; set; }
        /// <summary>
        /// so tien thanh toan
        /// </summary>
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.TotalMoney")]
        public string TotalMoney { get; set; }
        public IList<ContractPaymentModel> lstContractPayment { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.File")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditure.Fields.CurrencyId")]
        public int? CurrencyId { get; set; }
        public IList<int> SelectedListFileId { get; set; }
        public IList<SelectListItem> AvailableCurrency { get; set; }
    }
    public class PaymentExpenditureSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditureSearch.Fields.UnitId")]
        public Int32? UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.PaymentExpenditureSearch.Fields.KeySearch")]
        public string Keysearch { get; set; }
        public List<SelectListItem> AvailableUnit { get; set; }
    }
    public class PaymentExpenditureListModel : BasePagedListModel<PaymentExpenditureModel>
    {

    }
}
