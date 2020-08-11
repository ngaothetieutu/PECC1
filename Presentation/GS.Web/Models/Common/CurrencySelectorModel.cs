using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseGSModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public int CurrentCurrencyId { get; set; }
    }
}