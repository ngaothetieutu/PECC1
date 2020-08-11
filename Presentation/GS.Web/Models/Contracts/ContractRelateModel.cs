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
    public class ContractRelateModel : BaseGSEntityModel
    {
        public int Contract1Id { get; set; }
        public int Contract2Id { get; set; }
        public int DisplayOrder { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public partial class ContractRelateListModel : BasePagedListModel<ContractRelateModel>
    {

    }
    public partial class ContractRelateSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractRelate.Fields.Name")]
        public string Name { get; set; }
    }
}
