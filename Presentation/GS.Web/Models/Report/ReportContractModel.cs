using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Models.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Contract = GS.Core.Domain.Contracts.Contract;

namespace GS.Web.Models.Report
{
    public class ReportContractModel : BaseGSEntityModel
    {
    }
    public class ReportSearchModel
    {
        public ReportSearchModel()
        {
            AvailableContractForm = new List<SelectListItem>();
            ConstructionName = new List<SelectListItem>();
            SelectConstructionIds = new List<int>();
            ConstructionTypeSLI = new List<SelectListItem>();
        }

        public string ConstructionCode { get; set; }
        public List<SelectListItem> ConstructionName { get; set; }
        public string ContractNameCode { get; set; }
        [UIHint("DateNullable")]
        public DateTime? StartDateFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? StartDateTo { get; set; }
        public string ConstructionType { get; set; }
        public Int32 ContractFormId { get; set; }
        public List<SelectListItem> AvailableContractForm { get; set; }
        public IList<int> SelectConstructionIds { get; set; }
        public string StringConstructionIds { get; set; }
        public ContractStatus contractStatus { get; set; }
        public SelectList AvailableContractStatus { get; set; }
        public List<SelectListItem> ConstructionTypeSLI { get; set; }
        public IList<int> SelectedConstructionType { get; set; }
    }
    public class ReportContractPaymentAcceptanceSearchModel
    {
        public ReportContractPaymentAcceptanceSearchModel()
        {
            AvailableunitCode = new List<SelectListItem>();
            SelectConstructionIds = new List<SelectListItem>();
            ConstructionIds = new List<int>();
            AvailableQuarter = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
        }
        public string StringConstructionIds { get; set; }
        public string contractCodeName { get; set; }
        [UIHint("DateNullable")]
        public DateTime? dateTimeFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeTo { get; set; }
        public List<SelectListItem> AvailableunitCode { get; set; }
        public Int32 unitCode { get; set; }
        public List<SelectListItem> SelectConstructionIds { get; set; }
        public IList<int> ConstructionIds { get; set; }
        public int quarterId { get; set; }
        public List<SelectListItem> AvailableQuarter { get; set; }
        public int yearId { get; set; }
        public List<SelectListItem> AvailableYear { get; set; }
    }
    public class ReportContractBBSearchModel
    {
        public ReportContractBBSearchModel()
        {
            ConstructionIds = new List<SelectListItem>();
            SelectedConstructionIds = new List<int>();
        }
        public string contractCodeName { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeTo { get; set; }
        public string constructionType { get; set; }
        public List<SelectListItem> ConstructionIds { get; set; }
        public IList<int> SelectedConstructionIds { get; set; }
        public string stringConstructionIds { get; set; }
    }
    public class ReportContractABSearchModel
    {
        public ReportContractABSearchModel()
        {
            SelectedConstructionIds = new List<int>();
            ConstructionIds = new List<SelectListItem>();
        }
        public string contractCodeName { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeTo { get; set; }
        public string constructionType { get; set; }
        public IList<int> SelectedConstructionIds { get; set; }
        public List<SelectListItem> ConstructionIds { get; set; }
        public string stringConstructionIds { get; set; }
    }
    public class ReportProcuringAgencySearchModel
    {
        public ReportProcuringAgencySearchModel()
        {
            SelectedProcuringAgencyIds = new List<int>();
            ProcuringAgencyCode = new List<SelectListItem>();
        }
        public IList<int> SelectedProcuringAgencyIds { get; set; }
        public List<SelectListItem> ProcuringAgencyCode { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeTo { get; set; }
        public string StringProcuringAgencyId { get; set; }
    }
    public class ReportContractDealineSearchModel
    {
        public ReportContractDealineSearchModel()
        {
            SelectedConstructionIds = new List<int>();
        }
        public IList<int> SelectedConstructionIds { get; set; }
        public List<SelectListItem> ConstructionIds { get; set; }
        public string stringConstructionIds { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? datetimeTo { get; set; }
    }
    public class ReportTaskByUniySearchModel
    {
        public ReportTaskByUniySearchModel()
        {
            SelectedConstructionIds = new List<int>();
            SelectedUnitIds = new List<int>();
            UnitIds = new List<SelectListItem>();
        }
        public IList<int> SelectedConstructionIds { get; set; }
        public List<SelectListItem> ConstructionIds { get; set; }
        public IList<int> SelectedUnitIds { get; set; }
        public List<SelectListItem> UnitIds { get; set; }
        public string stringConstructionIds { get; set; }
        public string stringUnitIds { get; set; }
        [UIHint("DateNullable")]
        public DateTime? dateFrom { get; set; }
        [UIHint("DateNullable")]
        public DateTime? dateTo { get; set; }
    }
    public class ReportContractUnfinishSearchModel
    {
        public ReportContractUnfinishSearchModel()
        {
            SelectedConstructionIds = new List<int>();
        }
        public IList<int> SelectedConstructionIds { get; set; }
        public List<SelectListItem> ConstructionIds { get; set; }
        [UIHint("DateNullable")]
        public DateTime? time { get; set; }
        public string stringConstructionIds { get; set; }
    }
}
