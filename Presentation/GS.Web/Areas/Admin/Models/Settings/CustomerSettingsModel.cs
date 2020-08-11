using System.ComponentModel.DataAnnotations;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a customer settings model
    /// </summary>
    public partial class CustomerSettingsModel : BaseGSModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernamesEnabled")]
        public bool UsernamesEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowUsersToChangeUsernames")]
        public bool AllowUsersToChangeUsernames { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CheckUsernameAvailabilityEnabled")]
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationEnabled")]
        public bool UsernameValidationEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationUseRegex")]
        public bool UsernameValidationUseRegex { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UsernameValidationRule")]
        public string UsernameValidationRule { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UserRegistrationType")]
        public int UserRegistrationType { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToUploadAvatars")]
        public bool AllowCustomersToUploadAvatars { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultAvatarEnabled")]
        public bool DefaultAvatarEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ShowCustomersLocation")]
        public bool ShowCustomersLocation { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ShowCustomersJoinDate")]
        public bool ShowCustomersJoinDate { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowViewingProfiles")]
        public bool AllowViewingProfiles { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NotifyNewCustomerRegistration")]
        public bool NotifyNewCustomerRegistration { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.RequireRegistrationForDownloadableProducts")]
        public bool RequireRegistrationForDownloadableProducts { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AllowCustomersToCheckGiftCardBalance")]
        public bool AllowCustomersToCheckGiftCardBalance { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideDownloadableProductsTab")]
        public bool HideDownloadableProductsTab { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideBackInStockSubscriptionsTab")]
        public bool HideBackInStockSubscriptionsTab { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CustomerNameFormat")]
        public int CustomerNameFormat { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordMinLength")]
        public int PasswordMinLength { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.UnduplicatedPasswordsNumber")]
        public int UnduplicatedPasswordsNumber { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordRecoveryLinkDaysValid")]
        public int PasswordRecoveryLinkDaysValid { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DefaultPasswordFormat")]
        public int DefaultPasswordFormat { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PasswordLifetime")]
        public int PasswordLifetime { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FailedPasswordAllowedAttempts")]
        public int FailedPasswordAllowedAttempts { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FailedPasswordLockoutMinutes")]
        public int FailedPasswordLockoutMinutes { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterEnabled")]
        public bool NewsletterEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterTickedByDefault")]
        public bool NewsletterTickedByDefault { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.HideNewsletterBlock")]
        public bool HideNewsletterBlock { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.NewsletterBlockAllowToUnsubscribe")]
        public bool NewsletterBlockAllowToUnsubscribe { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StoreLastVisitedPage")]
        public bool StoreLastVisitedPage { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StoreIpAddresses")]
        public bool StoreIpAddresses { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.EnteringEmailTwice")]
        public bool EnteringEmailTwice { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.GenderEnabled")]
        public bool GenderEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthEnabled")]
        public bool DateOfBirthEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthRequired")]
        public bool DateOfBirthRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.DateOfBirthMinimumAge")]
        [UIHint("Int32Nullable")]
        public int? DateOfBirthMinimumAge { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CompanyEnabled")]
        public bool CompanyEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CompanyRequired")]
        public bool CompanyRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddressEnabled")]
        public bool StreetAddressEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddressRequired")]
        public bool StreetAddressRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddress2Enabled")]
        public bool StreetAddress2Enabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StreetAddress2Required")]
        public bool StreetAddress2Required { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ZipPostalCodeEnabled")]
        public bool ZipPostalCodeEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ZipPostalCodeRequired")]
        public bool ZipPostalCodeRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CityEnabled")]
        public bool CityEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CityRequired")]
        public bool CityRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountyEnabled")]
        public bool CountyEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountyRequired")]
        public bool CountyRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountryEnabled")]
        public bool CountryEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.CountryRequired")]
        public bool CountryRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StateProvinceEnabled")]
        public bool StateProvinceEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.StateProvinceRequired")]
        public bool StateProvinceRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FaxEnabled")]
        public bool FaxEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.FaxRequired")]
        public bool FaxRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AcceptPrivacyPolicyEnabled")]
        public bool AcceptPrivacyPolicyEnabled { get; set; }        

        #endregion
    }
}