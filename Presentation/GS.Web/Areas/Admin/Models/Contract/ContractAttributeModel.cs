using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Contract;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Contract
{
    [Validator(typeof(ContractAttributeValidator))]
    public partial class ContractAttributeModel : BaseGSEntityModel, ILocalizedModel<ContractAttributeLocalizedModel>
    {
        #region Ctor
        public ContractAttributeModel()
        {
            Locales = new List<ContractAttributeLocalizedModel>();
            ContractAttributeValueSearchModel = new ContractAttributeValueSearchModel();
        }
        #endregion

        #region Properties
        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.ContractAttributeld")]
        public int AttributeControlTypeId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.ContractAttributeName")]
        public string AttributeControlTypeName { get; set; }

        public IList<ContractAttributeLocalizedModel> Locales { get; set; }

        public ContractAttributeValueSearchModel ContractAttributeValueSearchModel { get; set; }
        #endregion
    }

    public partial class ContractAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}
