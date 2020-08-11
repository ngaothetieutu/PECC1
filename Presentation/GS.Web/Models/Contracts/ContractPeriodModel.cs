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
    [Validator(typeof(ContractPeriodValidator))]
    public class ContractPeriodModel: BaseGSEntityModel
    {
        public string Name { get; set; }

    }
    public partial class ContractPeriodListModel : BasePagedListModel<ContractPeriodModel>
    {

    }
    public partial class ContractPeriodSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractPeriod.Fields.Name")]
        public string Name { get; set; }
    }
}
