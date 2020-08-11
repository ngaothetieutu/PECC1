using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Cms
{
    public partial class RenderWidgetModel : BaseGSModel
    {
        public string WidgetViewComponentName { get; set; }
        public object WidgetViewComponentArguments { get; set; }
    }
}