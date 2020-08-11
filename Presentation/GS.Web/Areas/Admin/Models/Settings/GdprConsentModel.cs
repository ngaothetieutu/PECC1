using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Settings;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using System.Collections.Generic;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a GDPR consent model
    /// </summary>
    [Validator(typeof(GdprConsentValidator))]
    public partial class GdprConsentModel : BaseGSEntityModel, ILocalizedModel<GdprConsentLocalizedModel>
    {
        #region Ctor

        public GdprConsentModel()
        {
            Locales = new List<GdprConsentLocalizedModel>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.Message")]
        public string Message { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.IsRequired")]
        public bool IsRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.RequiredMessage")]
        public string RequiredMessage { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayDuringRegistration")]
        public bool DisplayDuringRegistration { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayOnCustomerInfoPage")]
        public bool DisplayOnCustomerInfoPage { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<GdprConsentLocalizedModel> Locales { get; set; }

        #endregion
    }
}