using FluentValidation.Attributes;
using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ConstructionModel : BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.TypeId")]
        public int TypeId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.CapitalId")]
        public int CapitalId { get; set; }
        public string TypeName { get; set; }
        public string CapitalName { get; set; }
        public IList<SelectListItem> AvailableTypes { get; set; }
        public IList<SelectListItem> AvailableCapitals { get; set; }
        public int ContractCount { get; set; }
        public string TotalMoneyContract { get; set; }
    }
    public partial class ConstructionListModel : BasePagedListModel<ConstructionModel>
    {

    }
    public partial class ConstructionSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.TypeId")]
        public int TypeId { get; set; }
        public IList<SelectListItem> lsType { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.Construction.Fields.Name")]
        public string Name { get; set; }
    }
    public class ReportConstruction
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string procuringAgency { get; set; }
        public string ContractName { get; set; }
        public string Type { get; set; }
        public bool IsEVN { get; set; }
        public Decimal? TotalAmount { get; set; }
        public string ContractForm { get; set; }
        public string ContractCode { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Decimal AdvanceAmount { get; set; }
        public Decimal ProgressAcceptance { get; set; }
        public Decimal ProgressPaymentAcceptance { get; set; }
        public int Status { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
