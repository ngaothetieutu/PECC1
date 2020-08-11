using System;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a GDPR log (request) model
    /// </summary>
    public partial class GdprLogModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.Customers.GdprLog.Fields.CustomerInfo")]
        public string CustomerInfo { get; set; }

        [GSResourceDisplayName("Admin.Customers.GdprLog.Fields.RequestType")]
        public string RequestType { get; set; }

        [GSResourceDisplayName("Admin.Customers.GdprLog.Fields.RequestDetails")]
        public string RequestDetails { get; set; }

        [GSResourceDisplayName("Admin.Customers.GdprLog.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}