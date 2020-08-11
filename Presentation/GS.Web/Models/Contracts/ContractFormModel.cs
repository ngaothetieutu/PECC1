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
    [Validator(typeof(ContractFormValidator))]
    public class ContractFormModel: BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractForm.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractForm.Fields.Description")]
        public string Description { get; set; }

    }
    public partial class ContractFormListModel : BasePagedListModel<ContractFormModel>
    {

    }
    public partial class ContractFormSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractForm.Fields.Name")]
        public string Name { get; set; }
    }
}
