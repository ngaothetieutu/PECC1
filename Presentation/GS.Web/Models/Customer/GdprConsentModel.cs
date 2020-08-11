using GS.Web.Framework.Models;

namespace GS.Web.Models.Customer
{
    public partial class GdprConsentModel : BaseGSEntityModel
    {
        public string Message { get; set; }

        public bool IsRequired { get; set; }

        public string RequiredMessage { get; set; }

        public bool Accepted { get; set; }
    }
}