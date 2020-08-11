using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Messages
{
    /// <summary>
    /// Represents a campaign search model
    /// </summary>
    public partial class CampaignSearchModel : BaseSearchModel
    {
        #region Ctor

        public CampaignSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Promotions.Campaigns.List.Stores")]
        public int StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}