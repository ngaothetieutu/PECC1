using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Represents a nopCommerce news model
    /// </summary>
    public partial class GSCommerceNewsModel : BaseGSModel
    {
        #region Ctor

        public GSCommerceNewsModel()
        {
            Items = new List<GSCommerceNewsDetailsModel>();
        }

        #endregion

        #region Properties

        public List<GSCommerceNewsDetailsModel> Items { get; set; }

        public bool HasNewItems { get; set; }

        public bool HideAdvertisements { get; set; }

        #endregion
    }
}