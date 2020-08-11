using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Customers;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer attribute model
    /// </summary>
    [Validator(typeof(CustomerAttributeValidator))]
    public partial class CustomerAttributeModel : BaseGSEntityModel, ILocalizedModel<CustomerAttributeLocalizedModel>
    {
        #region Ctor

        public CustomerAttributeModel()
        {
            Locales = new List<CustomerAttributeLocalizedModel>();
            CustomerAttributeValueSearchModel = new CustomerAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        public IList<CustomerAttributeLocalizedModel> Locales { get; set; }

        public CustomerAttributeValueSearchModel CustomerAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class CustomerAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}