using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class CurrencyModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public string CurrencySymbol { get; set; }
    }
}