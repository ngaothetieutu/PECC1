using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an address settings model
    /// </summary>
    public partial class AddressSettingsModel : BaseGSModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CompanyEnabled")]
        public bool CompanyEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CompanyRequired")]
        public bool CompanyRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.StreetAddressEnabled")]
        public bool StreetAddressEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.StreetAddressRequired")]
        public bool StreetAddressRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.StreetAddress2Enabled")]
        public bool StreetAddress2Enabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.StreetAddress2Required")]
        public bool StreetAddress2Required { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.ZipPostalCodeEnabled")]
        public bool ZipPostalCodeEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.ZipPostalCodeRequired")]
        public bool ZipPostalCodeRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CityEnabled")]
        public bool CityEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CityRequired")]
        public bool CityRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CountyEnabled")]
        public bool CountyEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CountyRequired")]
        public bool CountyRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.CountryEnabled")]
        public bool CountryEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.StateProvinceEnabled")]
        public bool StateProvinceEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.FaxEnabled")]
        public bool FaxEnabled { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Settings.CustomerUser.AddressFormFields.FaxRequired")]
        public bool FaxRequired { get; set; }

        #endregion
    }
}