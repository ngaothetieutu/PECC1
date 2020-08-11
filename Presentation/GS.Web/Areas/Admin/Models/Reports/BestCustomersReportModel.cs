using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a best customers report model
    /// </summary>
    public partial class BestCustomersReportModel : BaseGSModel
    {
        #region Properties

        public int CustomerId { get; set; }

        [GSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.Customer")]
        public string CustomerName { get; set; }

        [GSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.OrderTotal")]
        public string OrderTotal { get; set; }

        [GSResourceDisplayName("Admin.Reports.Customers.BestBy.Fields.OrderCount")]
        public decimal OrderCount { get; set; }
        
        #endregion
    }
}