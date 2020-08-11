using System.Collections.Generic;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Security;

namespace GS.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord AllowCustomerImpersonation = new PermissionRecord { Name = "Admin area. Allow Customer Impersonation", SystemName = "AllowCustomerImpersonation", Category = "Customers" };
        public static readonly PermissionRecord ManageProducts = new PermissionRecord { Name = "Admin area. Manage Products", SystemName = "ManageProducts", Category = "Catalog" };
        public static readonly PermissionRecord ManageCategories = new PermissionRecord { Name = "Admin area. Manage Categories", SystemName = "ManageCategories", Category = "Catalog" };
        public static readonly PermissionRecord ManageManufacturers = new PermissionRecord { Name = "Admin area. Manage Manufacturers", SystemName = "ManageManufacturers", Category = "Catalog" };
        public static readonly PermissionRecord ManageProductReviews = new PermissionRecord { Name = "Admin area. Manage Product Reviews", SystemName = "ManageProductReviews", Category = "Catalog" };
        public static readonly PermissionRecord ManageProductTags = new PermissionRecord { Name = "Admin area. Manage Product Tags", SystemName = "ManageProductTags", Category = "Catalog" };
        public static readonly PermissionRecord ManageAttributes = new PermissionRecord { Name = "Admin area. Manage Attributes", SystemName = "ManageAttributes", Category = "Catalog" };
        public static readonly PermissionRecord ManageCustomers = new PermissionRecord { Name = "Admin area. Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };
        public static readonly PermissionRecord ManageVendors = new PermissionRecord { Name = "Admin area. Manage Vendors", SystemName = "ManageVendors", Category = "Customers" };
        public static readonly PermissionRecord ManageCurrentCarts = new PermissionRecord { Name = "Admin area. Manage Current Carts", SystemName = "ManageCurrentCarts", Category = "Orders" };
        public static readonly PermissionRecord ManageOrders = new PermissionRecord { Name = "Admin area. Manage Orders", SystemName = "ManageOrders", Category = "Orders" };
        public static readonly PermissionRecord ManageRecurringPayments = new PermissionRecord { Name = "Admin area. Manage Recurring Payments", SystemName = "ManageRecurringPayments", Category = "Orders" };
        public static readonly PermissionRecord ManageGiftCards = new PermissionRecord { Name = "Admin area. Manage Gift Cards", SystemName = "ManageGiftCards", Category = "Orders" };
        public static readonly PermissionRecord ManageReturnRequests = new PermissionRecord { Name = "Admin area. Manage Return Requests", SystemName = "ManageReturnRequests", Category = "Orders" };
        public static readonly PermissionRecord OrderCountryReport = new PermissionRecord { Name = "Admin area. Access order country report", SystemName = "OrderCountryReport", Category = "Orders" };
        public static readonly PermissionRecord ManageAffiliates = new PermissionRecord { Name = "Admin area. Manage Affiliates", SystemName = "ManageAffiliates", Category = "Promo" };
        public static readonly PermissionRecord ManageCampaigns = new PermissionRecord { Name = "Admin area. Manage Campaigns", SystemName = "ManageCampaigns", Category = "Promo" };
        public static readonly PermissionRecord ManageDiscounts = new PermissionRecord { Name = "Admin area. Manage Discounts", SystemName = "ManageDiscounts", Category = "Promo" };
        public static readonly PermissionRecord ManageNewsletterSubscribers = new PermissionRecord { Name = "Admin area. Manage Newsletter Subscribers", SystemName = "ManageNewsletterSubscribers", Category = "Promo" };
        public static readonly PermissionRecord ManagePolls = new PermissionRecord { Name = "Admin area. Manage Polls", SystemName = "ManagePolls", Category = "Content Management" };
        public static readonly PermissionRecord ManageNews = new PermissionRecord { Name = "Admin area. Manage News", SystemName = "ManageNews", Category = "Content Management" };
        public static readonly PermissionRecord ManageBlog = new PermissionRecord { Name = "Admin area. Manage Blog", SystemName = "ManageBlog", Category = "Content Management" };
        public static readonly PermissionRecord ManageWidgets = new PermissionRecord { Name = "Admin area. Manage Widgets", SystemName = "ManageWidgets", Category = "Content Management" };
        public static readonly PermissionRecord ManageTopics = new PermissionRecord { Name = "Admin area. Manage Topics", SystemName = "ManageTopics", Category = "Content Management" };
        public static readonly PermissionRecord ManageForums = new PermissionRecord { Name = "Admin area. Manage Forums", SystemName = "ManageForums", Category = "Content Management" };
        public static readonly PermissionRecord ManageMessageTemplates = new PermissionRecord { Name = "Admin area. Manage Message Templates", SystemName = "ManageMessageTemplates", Category = "Content Management" };
        public static readonly PermissionRecord ManageCountries = new PermissionRecord { Name = "Admin area. Manage Countries", SystemName = "ManageCountries", Category = "Configuration" };
        public static readonly PermissionRecord ManageLanguages = new PermissionRecord { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };
        public static readonly PermissionRecord ManageSettings = new PermissionRecord { Name = "Admin area. Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManagePaymentMethods = new PermissionRecord { Name = "Admin area. Manage Payment Methods", SystemName = "ManagePaymentMethods", Category = "Configuration" };
        public static readonly PermissionRecord ManageExternalAuthenticationMethods = new PermissionRecord { Name = "Admin area. Manage External Authentication Methods", SystemName = "ManageExternalAuthenticationMethods", Category = "Configuration" };
        public static readonly PermissionRecord ManageTaxSettings = new PermissionRecord { Name = "Admin area. Manage Tax Settings", SystemName = "ManageTaxSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManageShippingSettings = new PermissionRecord { Name = "Admin area. Manage Shipping Settings", SystemName = "ManageShippingSettings", Category = "Configuration" };
        public static readonly PermissionRecord ManageCurrencies = new PermissionRecord { Name = "Admin area. Manage Currencies", SystemName = "ManageCurrencies", Category = "Configuration" };
        public static readonly PermissionRecord ManageActivityLog = new PermissionRecord { Name = "Admin area. Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageAcl = new PermissionRecord { Name = "Admin area. Manage ACL", SystemName = "ManageACL", Category = "Configuration" };
        public static readonly PermissionRecord ManageEmailAccounts = new PermissionRecord { Name = "Admin area. Manage Email Accounts", SystemName = "ManageEmailAccounts", Category = "Configuration" };
        public static readonly PermissionRecord ManageStores = new PermissionRecord { Name = "Admin area. Manage Stores", SystemName = "ManageStores", Category = "Configuration" };
        public static readonly PermissionRecord ManagePlugins = new PermissionRecord { Name = "Admin area. Manage Plugins", SystemName = "ManagePlugins", Category = "Configuration" };
        public static readonly PermissionRecord ManageSystemLog = new PermissionRecord { Name = "Admin area. Manage System Log", SystemName = "ManageSystemLog", Category = "Configuration" };
        public static readonly PermissionRecord ManageMessageQueue = new PermissionRecord { Name = "Admin area. Manage Message Queue", SystemName = "ManageMessageQueue", Category = "Configuration" };
        public static readonly PermissionRecord ManageMaintenance = new PermissionRecord { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };
        public static readonly PermissionRecord HtmlEditorManagePictures = new PermissionRecord { Name = "Admin area. HTML Editor. Manage pictures", SystemName = "HtmlEditor.ManagePictures", Category = "Configuration" };
        public static readonly PermissionRecord ManageScheduleTasks = new PermissionRecord { Name = "Admin area. Manage Schedule Tasks", SystemName = "ManageScheduleTasks", Category = "Configuration" };
        
        //public store permissions
        public static readonly PermissionRecord DisplayPrices = new PermissionRecord { Name = "Public store. Display Prices", SystemName = "DisplayPrices", Category = "PublicStore" };
        public static readonly PermissionRecord EnableShoppingCart = new PermissionRecord { Name = "Public store. Enable shopping cart", SystemName = "EnableShoppingCart", Category = "PublicStore" };
        public static readonly PermissionRecord EnableWishlist = new PermissionRecord { Name = "Public store. Enable wishlist", SystemName = "EnableWishlist", Category = "PublicStore" };
        public static readonly PermissionRecord PublicStoreAllowNavigation = new PermissionRecord { Name = "Public store. Allow navigation", SystemName = "PublicStoreAllowNavigation", Category = "PublicStore" };
        public static readonly PermissionRecord AccessClosedStore = new PermissionRecord { Name = "Public store. Access a closed store", SystemName = "AccessClosedStore", Category = "PublicStore" };

        //App works
        public static readonly PermissionRecord AccessAppWorkPanel = new PermissionRecord { Name = "Access App Work area", SystemName = "AccessAppWorkPanel", Category = "Standard" };
        public static readonly PermissionRecord ManageConstructionType = new PermissionRecord { Name = "App Works area. Manage Construction Type", SystemName = "ManageConstructionType", Category = "Contract" };
        public static readonly PermissionRecord ManageConstruction = new PermissionRecord { Name = "App Works area. Manage Construction", SystemName = "ManageConstruction", Category = "Contract" };
        public static readonly PermissionRecord ManageConstructionCapital = new PermissionRecord { Name = "App Works area. Manage Construction Capital", SystemName = "ManageConstructionCapital", Category = "Contract" };
        public static readonly PermissionRecord ManageContractForm = new PermissionRecord { Name = "App Works area. Manage Contract Form", SystemName = "ManageContractForm", Category = "Contract" };
        public static readonly PermissionRecord ManageContractType = new PermissionRecord { Name = "App Works area. Manage Contract Type", SystemName = "ManageContractType", Category = "Contract" };
        public static readonly PermissionRecord ManageContractPeriod = new PermissionRecord { Name = "App Works area. Manage Contract Period", SystemName = "ManageContractPeriod", Category = "Contract" };
        public static readonly PermissionRecord ManageTaskGroup = new PermissionRecord { Name = "App Works area. Manage Task Group", SystemName = "ManageTaskGroup", Category = "Works" };
        public static readonly PermissionRecord ManageUnit = new PermissionRecord { Name = "App Works area. Manage Unit", SystemName = "ManageUnit", Category = "Contract" };
        public static readonly PermissionRecord ManageProcuringAgency = new PermissionRecord { Name = "App Works area. Manage ProcuringAgency", SystemName = "ManageProcuringAgency", Category = "Contract" };
        public static readonly PermissionRecord ManageContract = new PermissionRecord { Name = "App Works area. Manage Contract", SystemName = "ManageContract", Category = "Contract" };
        public static readonly PermissionRecord ManageTask = new PermissionRecord { Name = "App Works area. Manage Task", SystemName = "ManageTask", Category = "Task" };
        public static readonly PermissionRecord ManageAcceptance = new PermissionRecord { Name = "App Works area. Manage Acceptance", SystemName = "ManageAcceptance", Category = "Contract" };
        public static readonly PermissionRecord ManageAcceptanceCreate = new PermissionRecord { Name = "App Works area. Manage Acceptance Create", SystemName = "ManageAcceptanceCreate", Category = "Contract" };
        public static readonly PermissionRecord ManageAcceptanceList = new PermissionRecord { Name = "App Works area. Manage Acceptance List", SystemName = "ManageAcceptanceList", Category = "Contract" };
        public static readonly PermissionRecord ManageAcceptanceEdit = new PermissionRecord { Name = "App Works area. Manage Acceptance Edit", SystemName = "ManageAcceptanceEdit", Category = "Contract" };
        public static readonly PermissionRecord ManagePaymentExpenditure = new PermissionRecord { Name = "App Works area. Manage Payment Expenditure", SystemName = "ManagePaymentExpenditure", Category = "Contract" };
        public static readonly PermissionRecord ManagePaymentExpenditureCreate = new PermissionRecord { Name = "App Works area. Manage Payment Expenditure Create", SystemName = "ManagePaymentExpenditureCreate", Category = "Contract" };
        public static readonly PermissionRecord ManagePaymentExpenditureList = new PermissionRecord { Name = "App Works area. Manage Payment Expenditure List", SystemName = "ManagePaymentExpenditureList", Category = "Contract" };
        public static readonly PermissionRecord ManageAdvance = new PermissionRecord { Name = "App Works area. ManageAdvance", SystemName = "ManageAdvance", Category = "Contract" };
        public static readonly PermissionRecord ManageAdvanceCreate = new PermissionRecord { Name = "App Works area. ManageAdvance Create", SystemName = "ManageAdvanceCreate", Category = "Contract" };
        public static readonly PermissionRecord ManageAdvanceList = new PermissionRecord { Name = "App Works area. ManageAdvance List", SystemName = "ManageAdvanceList", Category = "Contract" };
            
        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[] 
            {
                AccessAdminPanel,
                AllowCustomerImpersonation,
                ManageProducts,
                ManageCategories,
                ManageManufacturers,
                ManageProductReviews,
                ManageProductTags,
                ManageAttributes,
                ManageCustomers,
                ManageVendors,
                ManageCurrentCarts,
                ManageOrders,
                ManageRecurringPayments,
                ManageGiftCards,
                ManageReturnRequests,
                OrderCountryReport,
                ManageAffiliates,
                ManageCampaigns,
                ManageDiscounts,
                ManageNewsletterSubscribers,
                ManagePolls,
                ManageNews,
                ManageBlog,
                ManageWidgets,
                ManageTopics,
                ManageForums,
                ManageMessageTemplates,
                ManageCountries,
                ManageLanguages,
                ManageSettings,
                ManagePaymentMethods,
                ManageExternalAuthenticationMethods,
                ManageTaxSettings,
                ManageShippingSettings,
                ManageCurrencies,
                ManageActivityLog,
                ManageAcl,
                ManageEmailAccounts,
                ManageStores,
                ManagePlugins,
                ManageSystemLog,
                ManageMessageQueue,
                ManageMaintenance,
                HtmlEditorManagePictures,
                ManageScheduleTasks,
                DisplayPrices,
                EnableShoppingCart,
                EnableWishlist,
                PublicStoreAllowNavigation,
                AccessClosedStore,
                //app work
                AccessAppWorkPanel,
                ManageConstructionType,
                ManageConstruction,
                ManageConstructionCapital,
                ManageContractType,
                ManageContractForm,
                ManageContractPeriod,
                ManageTaskGroup,
                ManageUnit,
                ManageContract,
                ManageAcceptance,
                ManageAcceptanceCreate,
                ManageAcceptanceList,
                ManageAcceptanceEdit,
                ManagePaymentExpenditure,
                ManagePaymentExpenditureCreate,
                ManagePaymentExpenditureList,
                ManageAdvance,
                ManageAdvanceCreate,
                ManageAdvanceList

            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            return new[] 
            {
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = GSCustomerDefaults.AdministratorsRoleName,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel,
                        AllowCustomerImpersonation,
                        ManageProducts,
                        ManageCategories,
                        ManageManufacturers,
                        ManageProductReviews,
                        ManageProductTags,
                        ManageAttributes,
                        ManageCustomers,
                        ManageVendors,
                        ManageCurrentCarts,
                        ManageOrders,
                        ManageRecurringPayments,
                        ManageGiftCards,
                        ManageReturnRequests,
                        OrderCountryReport,
                        ManageAffiliates,
                        ManageCampaigns,
                        ManageDiscounts,
                        ManageNewsletterSubscribers,
                        ManagePolls,
                        ManageNews,
                        ManageBlog,
                        ManageWidgets,
                        ManageTopics,
                        ManageForums,
                        ManageMessageTemplates,
                        ManageCountries,
                        ManageLanguages,
                        ManageSettings,
                        ManagePaymentMethods,
                        ManageExternalAuthenticationMethods,
                        ManageTaxSettings,
                        ManageShippingSettings,
                        ManageCurrencies,
                        ManageActivityLog,
                        ManageAcl,
                        ManageEmailAccounts,
                        ManageStores,
                        ManagePlugins,
                        ManageSystemLog,
                        ManageMessageQueue,
                        ManageMaintenance,
                        HtmlEditorManagePictures,
                        ManageScheduleTasks,
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation,
                        AccessClosedStore
                    }
                },
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = GSCustomerDefaults.ForumModeratorsRoleName,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = GSCustomerDefaults.GuestsRoleName,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = GSCustomerDefaults.RegisteredRoleName,
                    PermissionRecords = new[] 
                    {
                        DisplayPrices,
                        EnableShoppingCart,
                        EnableWishlist,
                        PublicStoreAllowNavigation
                    }
                },
                new DefaultPermissionRecord 
                {
                    CustomerRoleSystemName = GSCustomerDefaults.VendorsRoleName,
                    PermissionRecords = new[] 
                    {
                        AccessAdminPanel,
                        ManageProducts,
                        ManageProductReviews,
                        ManageOrders
                    }
                }
            };
        }
    }
}