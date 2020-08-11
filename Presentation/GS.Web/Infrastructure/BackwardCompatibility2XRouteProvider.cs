﻿using Microsoft.AspNetCore.Routing;
using GS.Core.Data;
using GS.Core.Domain.Common;
using GS.Core.Infrastructure;
using GS.Web.Framework.Localization;
using GS.Web.Framework.Mvc.Routing;

namespace GS.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided routes used for backward compatibility with 2.x versions of nopCommerce
    /// </summary>
    public partial class BackwardCompatibility2XRouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            if (DataSettingsManager.DatabaseIsInstalled && !EngineContext.Current.Resolve<CommonSettings>().SupportPreviousGScommerceVersions)
                return;

            //products
            routeBuilder.MapLocalizedRoute("", "p/{productId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectProductById" });

            //categories
            routeBuilder.MapLocalizedRoute("", "c/{categoryId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectCategoryById" });

            //manufacturers
            routeBuilder.MapLocalizedRoute("", "m/{manufacturerId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectManufacturerById" });

            //news
            routeBuilder.MapLocalizedRoute("", "news/{newsItemId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectNewsItemById" });

            //blog
            routeBuilder.MapLocalizedRoute("", "blog/{blogPostId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectBlogPostById" });

            //topic
            routeBuilder.MapLocalizedRoute("", "t/{SystemName}",
                new { controller = "BackwardCompatibility2X", action = "RedirectTopicBySystemName" });

            //vendors
            routeBuilder.MapLocalizedRoute("", "vendor/{vendorId:min(0)}/{SeName?}",
                new { controller = "BackwardCompatibility2X", action = "RedirectVendorById" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => -1000; //register it after all other IRouteProvider are processed

        #endregion
    }
}