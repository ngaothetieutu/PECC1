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
    [Validator(typeof(ContractPaymentAcceptanceValidator))]
    public class ContractPaymentAcceptanceModel : BaseGSEntityModel
    {
        public ContractPaymentAcceptanceModel()
        {
            AvailableContractAcceptances = new List<SelectListItem>();
            SelectedContractAcceptanceIds = new List<int>();
            listPaymentAcceptanceSub = new List<ContractPaymentAcceptanceSubModel>();
            SelectedListFileId = new List<int>();
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.Name")]
        public String Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.Code")]
        public String Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.Description")]
        public String Description { get; set; }

        public Int32 CreatorId { get; set; }      
        public DateTime CreatedDate { get; set; }
        public Int32 ContractId { get; set; }
        //public virtual Contract contract { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ApprovalId")]
        public Int32? ApprovalId { get; set; }
        [UIHint("DateNullable")]
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ApprovalDate")]
        public DateTime? ApprovalDate { get; set; }      
        public Int32 StatusId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.TotalAmount")]
        public Int64 TotalAmount { get; set; }
        public ContractPaymentAcceptanceStatus contractPaymentAcceptancestatus
        {
            get => (ContractPaymentAcceptanceStatus)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ResponsibleId")]
        public Int32? ResponsibleId { get; set; }      
        public Int32? CurrencyId { get; set; }      
        public String DataInfo { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ReduceAdvance")]
        public Decimal ReduceAdvance { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ReduceKeep")]
        public Decimal ReduceKeep { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.ReduceOther")]
        public Decimal ReduceOther { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.Depreciation")]
        public Decimal? Depreciation { get; set; }
        //addmore 
        public string AmountText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.File")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        public IList<int> SelectedListFileId { get; set; }
        public string CurrencyName { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.AvailableContractAcceptances")]
        public List<SelectListItem> AvailableContractAcceptances { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractPaymentAcceptance.Fields.SelectedContractAcceptanceIds")]
        public IList<int> SelectedContractAcceptanceIds { get; set; }
        public List<ContractPaymentAcceptanceSubModel> listPaymentAcceptanceSub { get; set; }
    }
    public partial class ContractPaymentAcceptanceListModel : BasePagedListModel<ContractPaymentAcceptanceModel>
    {
      
    }
    public partial class ContractPaymentAcceptanceSearchModel : BaseSearchModel
    {
       public int ContractId { get; set; }
        public int TaskId { get; set; }
    }    
}
