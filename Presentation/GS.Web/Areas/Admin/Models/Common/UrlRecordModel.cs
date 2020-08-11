using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record model
    /// </summary>
    public partial class UrlRecordModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.System.SeNames.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.System.SeNames.EntityId")]
        public int EntityId { get; set; }

        [GSResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [GSResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [GSResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [GSResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }

        #endregion
    }
}