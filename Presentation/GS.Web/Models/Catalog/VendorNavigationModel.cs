using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Catalog
{
    public partial class VendorNavigationModel : BaseGSModel
    {
        public VendorNavigationModel()
        {
            this.Vendors = new List<VendorBriefInfoModel>();
        }

        public IList<VendorBriefInfoModel> Vendors { get; set; }

        public int TotalVendors { get; set; }
    }

    public partial class VendorBriefInfoModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }
    }
}