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
    [Validator(typeof(ContractAcceptanceSubValidator))]
    public class ContractAcceptanceSubModel : BaseGSEntityModel
    {
        public ContractAcceptanceSubModel()
        {
            AvailableUnits = new List<SelectListItem>();
            AvailableTasks = new List<SelectListItem>();
        }
        public Int32 AcceptanceId { get; set; }
        public Int32 ContractId { get; set; }
        public Int32 UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceSub.Fields.Description")]
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceSub.Fields.Ratio")]
        public Decimal? Ratio { get; set; }
        public int StatusId { get; set; }
        public ContractAcceptanceSubStatus contractAcceptanceSubStatus
        {
            get => (ContractAcceptanceSubStatus)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceSub.Fields.TotalAmount")]
        [UIHint("WorkCurrency")]
        public string TotalAmount { get; set; }
        [UIHint("WorkCurrency")]
        public string TotalMoney { get; set; }
        public Int32? CurrencyId { get; set; }
        public string TotalAmountText { get; set; }
        public string CurrencyName { get; set; }
        public Int32? TaskId { get; set; }
        public string TaskName { get; set; }
        public string UnitName { get; set; }
        public bool hasBB { get; set; }
        public int Type { get; set; }
        //add more
        /// <summary>
        /// gia tri cong viec
        /// </summary>
        public decimal TaskValue { get; set; }
        public string TaskValueText { get; set; }
        /// <summary>
        /// gia tri da nghiem thu
        /// </summary>
        public decimal TaskAcceptanced { get; set; }
        public string TaskAcceptancedText { get; set; }
        //public IList<SelectListItem> AvailableTaskGroup { get; set; }
        public IList<SelectListItem> AvailableUnits { get; set; }
        public IList<SelectListItem> AvailableTasks { get; set; }
        public string TotalAmountTask {get; set;}
        /// <summary>
        /// Task co BB hay khong? danh cho NTNB
        /// </summary>
        public bool HasBB { get; set; }
        public string ContractBBValueText { get; set; }
        public decimal ContractBBValue { get; set; }
        /// <summary>
        /// Ngay thuc hien nghiem thu noi bo
        /// </summary>
        public string DateAproval { get; set; }
        ///<sumary>
        ///Gia tri nghiem thu thanh toan sau thue cua cong viec
        ///</sumary>
        public string TotalPaymentAcceptanceTask { get; set; }
        ///<sumary>
        ///Gia tri nghiem thu thanh toan truoc thue cua cong viec
        ///</sumary>
        public string TotalPaymentAcceptanceTaskTruocThue { get; set; }

    }
    public partial class ContractAcceptanceSubListModel : BasePagedListModel<ContractAcceptanceSubModel>
    {

    }
    public partial class ContractAcceptanceSubSearchModel : BaseSearchModel
    {
        public int ContractId { get; set; }
        public int AcceptanceId { get; set; }
        public List<SelectListItem> AvailableUnit { get; set; }
    }
}
