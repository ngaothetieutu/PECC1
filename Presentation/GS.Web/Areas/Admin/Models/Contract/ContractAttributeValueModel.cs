using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Contract;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Contract
{
    [Validator(typeof(ContractAttributeValueValidator))]
    public partial class ContractAttributeValueModel : BaseGSEntityModel, ILocalizedModel<ContractAttributeValueLocalizedModel>
    {
        #region Ctor
        public ContractAttributeValueModel()
        {
            Locales = new List<ContractAttributeValueLocalizedModel>();
        }
        #endregion

        #region Properties
        public int ContractAttributeId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ContractAttributeValueLocalizedModel> Locales { get; set; }
        #endregion
    }

    public partial class ContractAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}
