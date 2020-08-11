using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Common
{
    public partial class CommonStatisticsModel : BaseGSModel
    {
        public int NumberOfOrders { get; set; }

        public int NumberOfCustomers { get; set; }

        public int NumberOfPendingReturnRequests { get; set; }

        public int NumberOfLowStockProducts { get; set; }
    }
}