using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseGSModel
    {
        public string ImpersonatedCustomerName { get; set; }
        public bool IsCustomerImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}