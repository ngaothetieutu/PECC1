﻿using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a customer user settings model
    /// </summary>
    public partial class CustomerUserSettingsModel : BaseGSModel, ISettingsModel
    {
        #region Ctor

        public CustomerUserSettingsModel()
        {
            CustomerSettings = new CustomerSettingsModel();
            AddressSettings = new AddressSettingsModel();
            DateTimeSettings = new DateTimeSettingsModel();
            ExternalAuthenticationSettings = new ExternalAuthenticationSettingsModel();
            CustomerAttributeSearchModel = new CustomerAttributeSearchModel();
            AddressAttributeSearchModel = new AddressAttributeSearchModel();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        public CustomerSettingsModel CustomerSettings { get; set; }

        public AddressSettingsModel AddressSettings { get; set; }

        public DateTimeSettingsModel DateTimeSettings { get; set; }

        public ExternalAuthenticationSettingsModel ExternalAuthenticationSettings { get; set; }

        public CustomerAttributeSearchModel CustomerAttributeSearchModel { get; set; }

        public AddressAttributeSearchModel AddressAttributeSearchModel { get; set; }

        #endregion
    }
}