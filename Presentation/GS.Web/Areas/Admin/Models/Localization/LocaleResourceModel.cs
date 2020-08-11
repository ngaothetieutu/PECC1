using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Localization;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource model
    /// </summary>
    [Validator(typeof(LanguageResourceValidator))]
    public partial class LocaleResourceModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        public string Value { get; set; }

        public int LanguageId { get; set; }

        #endregion
    }
}