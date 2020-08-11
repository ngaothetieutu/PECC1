using GS.Core.Domain.Tax;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class TaxTypeSelectorModel : BaseGSModel
    {
        public TaxDisplayType CurrentTaxType { get; set; }
    }
}