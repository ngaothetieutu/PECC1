using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Settings;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a setting model
    /// </summary>
    [Validator(typeof(SettingValidator))]
    public partial class SettingModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
        public string Value { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.StoreName")]
        public string Store { get; set; }

        public int StoreId { get; set; }

        #endregion
    }
}