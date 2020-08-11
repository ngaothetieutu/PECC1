using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a currency exchange rate provider model
    /// </summary>
    public partial class CurrencyExchangeRateProviderModel : BaseGSModel
    {
        #region Ctor

        public CurrencyExchangeRateProviderModel()
        {
            ExchangeRates = new List<CurrencyExchangeRateModel>();
            ExchangeRateProviders = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.CurrencyRateAutoUpdateEnabled")]
        public bool AutoUpdateEnabled { get; set; }

        public IList<CurrencyExchangeRateModel> ExchangeRates { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.ExchangeRateProvider")]
        public string ExchangeRateProvider { get; set; }
        public IList<SelectListItem> ExchangeRateProviders { get; set; }

        #endregion
    }
}