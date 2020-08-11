using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ProcuringAgencyModel : BaseGSEntityModel
    {
        public ProcuringAgencyModel()
        {
            this.procuringAgencyType = ProcuringAgencyType.Main;
        }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.TypeId")]
        public int TypeId { get; set; }
        public ProcuringAgencyType procuringAgencyType
        {
            get => (ProcuringAgencyType)TypeId;
            set => TypeId = (int)value;
        }
        public string TypeText { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.Representer")]
        public string Representer { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.Postion")]
        public String Postion { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.CompanyAddress")]
        public String CompanyAddress { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.TaxCode")]
        public String TaxCode { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.TaxInfo")]
        public String TaxInfo { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.CompanyFax")]
        public String CompanyFax { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.CompanyPhone")]
        public String CompanyPhone { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.CompanyEmail")]
        public String CompanyEmail { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.BankInfo")]
        public String BankInfo { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.IsInEVN")]
        public Boolean IsInEVN { get; set; }
        //[GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.IsInEVN")]
        //public string InEVN { get; set; }
        public SelectList lsType { get; set; }

        
        //public string TypeName { get; set; }
        //public string CapitalName { get; set; }
        //public IList<SelectListItem> AvailableTypes { get; set; }
        //public IList<SelectListItem> AvailableCapitals { get; set; }
    }
    public partial class ProcuringAgencyListModel : BasePagedListModel<ProcuringAgencyModel>
    {

    }
    public partial class ProcuringAgencySearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.TypeId")]
        public int TypeId { get; set; }
        public SelectList lsType { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ProcuringAgency.Fields.Name")]
        public string Name { get; set; }
        public Boolean IsInEVN { get; set; }

    }
}
