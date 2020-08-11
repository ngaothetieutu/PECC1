﻿using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Localization;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a language model
    /// </summary>
    [Validator(typeof(LanguageValidator))]
    public partial class LanguageModel : BaseGSEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public LanguageModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            LocaleResourceSearchModel = new LocaleResourceSearchModel();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        public string LanguageCulture { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        public string UniqueSeoCode { get; set; }
        
        //flags
        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        public string FlagImageFileName { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        //default currency
        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
        public int DefaultCurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        //store mapping
        [GSResourceDisplayName("Admin.Configuration.Languages.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        // search
        public LocaleResourceSearchModel LocaleResourceSearchModel { get; set; }

        #endregion
    }
}