using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Areas.Admin.Models.Contract;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Settings
{
    public partial class ContractAttributeSettingModel : BaseGSModel
    {
        #region Ctor
        public ContractAttributeSettingModel()
        {
            ContractAttributeSearchModel = new ContractAttributeSearchModel();
        }
        #endregion

        #region Properties
        public ContractAttributeSearchModel ContractAttributeSearchModel { get; set; }
        #endregion
    }
}
