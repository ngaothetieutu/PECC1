using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Areas.Admin.Models.WidgetApps
{
    public partial class WidgetAppModel : BaseGSEntityModel
    {
        #region Properties
        [GSResourceDisplayName("Admin.WidgetApps.WidgetApp.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("Admin.WidgetApps.WidgetApp.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("Admin.WidgetApps.WidgetApp.Fields.ViewId")]
        public string ViewId { get; set; }
        #endregion
    }
}
