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
    [Validator(typeof(ConstructionTypeValidator))]
    public class ConstructionTypeModel: BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ConstructionType.Fields.Name")]
        public string Name { get; set; }
    }
    public partial class ConstructionTypeListModel : BasePagedListModel<ConstructionTypeModel>
    {

    }
    public partial class ConstructionTypeSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ConstructionType.Fields.Name")]
        public string Name { get; set; }
    }
}
