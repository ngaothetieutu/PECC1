using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Areas.Admin.Models.Reports;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Represents a dashboard model
    /// </summary>
    public partial class DashboardModel : BaseGSModel
    {
        #region Ctor

        public DashboardModel()
        {
            this.PopularSearchTerms = new PopularSearchTermSearchModel();
            this.BestsellersByAmount = new BestsellerBriefSearchModel();
            this.BestsellersByQuantity = new BestsellerBriefSearchModel();
        }

        #endregion

        #region Properties

        public bool IsLoggedInAsVendor { get; set; }

        public PopularSearchTermSearchModel PopularSearchTerms { get; set; }

        public BestsellerBriefSearchModel BestsellersByAmount { get; set; }

        public BestsellerBriefSearchModel BestsellersByQuantity { get; set; }

        #endregion
    }
}