using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Templates;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a product template model
    /// </summary>
    [Validator(typeof(ProductTemplateValidator))]
    public partial class ProductTemplateModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.System.Templates.Product.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Product.ViewPath")]
        public string ViewPath { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Product.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Product.IgnoredProductTypes")]
        public string IgnoredProductTypes { get; set; }

        #endregion
    }
}