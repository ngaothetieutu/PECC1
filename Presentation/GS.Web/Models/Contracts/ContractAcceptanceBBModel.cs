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
    [Validator(typeof(ContractAcceptanceBBValidator))]
    public class ContractAcceptanceBBModel : BaseGSEntityModel
    {
        public ContractAcceptanceBBModel()
        {
            AvailableUnit = new List<SelectListItem>();
            SelectedListFileId = new List<int>(); 
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.Code")]
        public String Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.Name")]
        public String Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.Description")]
        public String Description { get; set; }      
        public int TypeId { get; set; }
        public int? CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.Ratio")]
        public decimal? Ratio { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.TotalAmount")]
        public decimal? TotalAmount { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.TaskId")]
        public int? TaskId { get; set; }
        public int ContractId { get; set; }
        public int ContractIdBB { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.ApprovalDate")]
        [UIHint("DateNullable")]
        public DateTime? ApprovalDate { get; set; }
        public int ApprovalId { get; set; }
        public int StatusId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.UnitId")]
        public int UnitId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.CurrencyId")]
        public int? CurrencyId { get; set; }
        //add more
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.File")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractAcceptanceBB.Fields.TaskName")]
        public string TaskName { get; set; }
        public IList<SelectListItem> AvailableUnit { get; set; }
        public IList<int> SelectedListFileId { get; set; }
    }
    public partial class ContractAcceptanceBBListModel : BasePagedListModel<ContractAcceptanceBBModel>
    {
        
    }
    public partial class ContractSearchModel : BaseSearchModel
    {      

    }
}
