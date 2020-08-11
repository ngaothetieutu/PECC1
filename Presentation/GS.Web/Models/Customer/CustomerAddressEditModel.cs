using GS.Web.Framework.Models;
using GS.Web.Models.Common;

namespace GS.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : BaseGSModel
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        
        public AddressModel Address { get; set; }
    }
}