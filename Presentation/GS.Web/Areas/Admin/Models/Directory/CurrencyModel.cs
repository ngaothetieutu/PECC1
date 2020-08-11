using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Directory;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a currency model
    /// </summary>
    [Validator(typeof(CurrencyValidator))]
    public partial class CurrencyModel : BaseGSEntityModel, ILocalizedModel<CurrencyLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public CurrencyModel()
        {
            Locales = new List<CurrencyLocalizedModel>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.CurrencyCode")]
        public string CurrencyCode { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayLocale")]
        public string DisplayLocale { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.Rate")]
        public decimal Rate { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.CustomFormatting")]
        public string CustomFormatting { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.Published")]
        public bool Published { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryExchangeRateCurrency")]
        public bool IsPrimaryExchangeRateCurrency { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryStoreCurrency")]
        public bool IsPrimaryStoreCurrency { get; set; }

        public IList<CurrencyLocalizedModel> Locales { get; set; }

        //store mapping
        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.RoundingType")]
        public int RoundingTypeId { get; set; }

        #endregion
    }

    public partial class CurrencyLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]
        public string Name { get; set; }
    }
}