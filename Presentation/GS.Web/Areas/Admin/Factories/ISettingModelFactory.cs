﻿using GS.Core.Domain.Gdpr;
using GS.Web.Areas.Admin.Models.Settings;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the setting model factory
    /// </summary>
    public partial interface ISettingModelFactory
    {
        /// <summary>
        /// Prepare blog settings model
        /// </summary>
        /// <returns>Blog settings model</returns>
        BlogSettingsModel PrepareBlogSettingsModel();

        /// <summary>
        /// Prepare vendor settings model
        /// </summary>
        /// <returns>Vendor settings model</returns>
        VendorSettingsModel PrepareVendorSettingsModel();

        /// <summary>
        /// Prepare forum settings model
        /// </summary>
        /// <returns>Forum settings model</returns>
        ForumSettingsModel PrepareForumSettingsModel();

        /// <summary>
        /// Prepare news settings model
        /// </summary>
        /// <returns>News settings model</returns>
        NewsSettingsModel PrepareNewsSettingsModel();

        /// <summary>
        /// Prepare tax settings model
        /// </summary>
        /// <returns>Tax settings model</returns>
        TaxSettingsModel PrepareTaxSettingsModel();

        /// <summary>
        /// Prepare catalog settings model
        /// </summary>
        /// <returns>Catalog settings model</returns>
        CatalogSettingsModel PrepareCatalogSettingsModel();
        
        /// <summary>
        /// Prepare paged sort option list model
        /// </summary>
        /// <param name="searchModel">Sort option search model</param>
        /// <returns>Sort option list model</returns>
        SortOptionListModel PrepareSortOptionListModel(SortOptionSearchModel searchModel);

      

        /// <summary>
        /// Prepare media settings model
        /// </summary>
        /// <returns>Media settings model</returns>
        MediaSettingsModel PrepareMediaSettingsModel();

        /// <summary>
        /// Prepare customer user settings model
        /// </summary>
        /// <returns>Customer user settings model</returns>
        CustomerUserSettingsModel PrepareCustomerUserSettingsModel();

        /// <summary>
        /// Prepare GDPR settings model
        /// </summary>
        /// <returns>GDPR settings model</returns>
        GdprSettingsModel PrepareGdprSettingsModel();

        /// <summary>
        /// Prepare paged GDPR consent list model
        /// </summary>
        /// <param name="searchModel">GDPR search model</param>
        /// <returns>GDPR consent list model</returns>
       GdprConsentListModel PrepareGdprConsentListModel(GdprConsentSearchModel searchModel);

        /// <summary>
        /// Prepare GDPR consent model
        /// </summary>
        /// <param name="model">GDPR consent model</param>
        /// <param name="gdprConsent">GDPR consent</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>GDPR consent model</returns>
        GdprConsentModel PrepareGdprConsentModel(GdprConsentModel model, GdprConsent gdprConsent, bool excludeProperties = false);

        /// <summary>
        /// Prepare general and common settings model
        /// </summary>
        /// <returns>General and common settings model</returns>
        GeneralCommonSettingsModel PrepareGeneralCommonSettingsModel();
        
    

        /// <summary>
        /// Prepare setting search model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting search model</returns>
        SettingSearchModel PrepareSettingSearchModel(SettingSearchModel searchModel);

        /// <summary>
        /// Prepare paged setting list model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting list model</returns>
        SettingListModel PrepareSettingListModel(SettingSearchModel searchModel);

        /// <summary>
        /// Prepare setting mode model
        /// </summary>
        /// <param name="modeName">Mode name</param>
        /// <returns>Setting mode model</returns>
        SettingModeModel PrepareSettingModeModel(string modeName);

        /// <summary>
        /// Prepare store scope configuration model
        /// </summary>
        /// <returns>Store scope configuration model</returns>
        StoreScopeConfigurationModel PrepareStoreScopeConfigurationModel();
    }
}