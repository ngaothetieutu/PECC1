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
    [Validator(typeof(ContractTypeValidator))]
    public class ContractTypeModel: BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractType.Fields.Name")]
        public string Name { get; set; }
        //add more
        /// <summary>
        /// so tien cua cac loai hop dong , su dung cho tung hop dong
        /// </summary>
        public string totalMoneyText;
    }
    public partial class ContractTypeListModel : BasePagedListModel<ContractTypeModel>
    {

    }
    public partial class ContractTypeSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractType.Fields.Name")]
        public string Name { get; set; }
    }
}
