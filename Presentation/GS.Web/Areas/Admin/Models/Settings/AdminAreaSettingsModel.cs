using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an admin area settings model
    /// </summary>
    public partial class AdminAreaSettingsModel : BaseGSModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminArea.UseRichEditorInMessageTemplates")]
        public bool UseRichEditorInMessageTemplates { get; set; }
        public bool UseRichEditorInMessageTemplates_OverrideForStore { get; set; }

        #endregion
    }
}