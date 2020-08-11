using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ConstructionCapitalValidator))]
    public class ConstructionCapitalModel : BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ConstructionCapital.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ConstructionCapital.Fields.Description")]
        public string Description { get; set; }
    }
    public partial class ConstructionCapitalListModel : BasePagedListModel<ConstructionCapitalModel>
    {

    }
    public partial class ConstructionCapitalSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ConstructionCapital.Fields.Name")]
        public string Name { get; set; }
    }
}
