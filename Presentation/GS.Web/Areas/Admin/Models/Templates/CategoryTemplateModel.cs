using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Templates;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a category template model
    /// </summary>
    [Validator(typeof(CategoryTemplateValidator))]
    public partial class CategoryTemplateModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.System.Templates.Category.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Category.ViewPath")]
        public string ViewPath { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Category.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}