using System.Collections.Generic;
using GS.Web.Framework.Models;
using GS.Web.Models.Common;

namespace GS.Web.Models.Customer
{
    public partial class CustomerAddressListModel : BaseGSModel
    {
        public CustomerAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
    }
}