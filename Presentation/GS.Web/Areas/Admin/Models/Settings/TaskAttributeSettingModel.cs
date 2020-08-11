using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Areas.Admin.Models.Tasks;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Settings
{
    public partial class TaskAttributeSettingModel : BaseGSModel
    {
        #region Ctor
        public TaskAttributeSettingModel()
        {
            TaskAttributeSearchModel = new TaskAttributeSearchModel();
        }
        #endregion

        #region Properties
        public TaskAttributeSearchModel TaskAttributeSearchModel { get; set; }
        #endregion
    }
}
