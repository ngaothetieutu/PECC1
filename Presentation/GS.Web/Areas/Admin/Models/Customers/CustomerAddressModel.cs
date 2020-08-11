using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer address model
    /// </summary>
    public partial class CustomerAddressModel : BaseGSModel
    {
        #region Ctor

        public CustomerAddressModel()
        {
            this.Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int CustomerId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}