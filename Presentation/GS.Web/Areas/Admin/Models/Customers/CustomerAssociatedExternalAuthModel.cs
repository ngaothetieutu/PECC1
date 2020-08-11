using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer associated external authentication model
    /// </summary>
    public partial class CustomerAssociatedExternalAuthModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.Email")]
        public string Email { get; set; }

        [GSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.ExternalIdentifier")]
        public string ExternalIdentifier { get; set; }
        
        [GSResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.AuthMethodName")]
        public string AuthMethodName { get; set; }

        #endregion
    }
}