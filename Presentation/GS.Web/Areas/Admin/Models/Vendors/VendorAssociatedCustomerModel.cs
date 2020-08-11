using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor associated customer model
    /// </summary>
    public partial class VendorAssociatedCustomerModel : BaseGSEntityModel
    {
        #region Properties

        public string Email { get; set; }

        #endregion
    }
}