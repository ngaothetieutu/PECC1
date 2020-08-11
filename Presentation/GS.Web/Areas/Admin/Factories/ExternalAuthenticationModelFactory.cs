﻿using System;
using System.Linq;
using GS.Services.Authentication.External;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.ExternalAuthentication;
using GS.Web.Framework.Extensions;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the external authentication method model factory implementation
    /// </summary>
    public partial class ExternalAuthenticationMethodModelFactory : IExternalAuthenticationMethodModelFactory
    {
        #region Fields

        private readonly IExternalAuthenticationService _externalAuthenticationService;

        #endregion

        #region Ctor

        public ExternalAuthenticationMethodModelFactory(IExternalAuthenticationService externalAuthenticationService)
        {
            this._externalAuthenticationService = externalAuthenticationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare external authentication method search model
        /// </summary>
        /// <param name="searchModel">External authentication method search model</param>
        /// <returns>External authentication method search model</returns>
        public virtual ExternalAuthenticationMethodSearchModel PrepareExternalAuthenticationMethodSearchModel(
            ExternalAuthenticationMethodSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged external authentication method list model
        /// </summary>
        /// <param name="searchModel">External authentication method search model</param>
        /// <returns>External authentication method list model</returns>
        public virtual ExternalAuthenticationMethodListModel PrepareExternalAuthenticationMethodListModel(
            ExternalAuthenticationMethodSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get external authentication methods
            var externalAuthenticationMethods = _externalAuthenticationService.LoadAllExternalAuthenticationMethods();

            //prepare grid model
            var model = new ExternalAuthenticationMethodListModel
            {
                Data = externalAuthenticationMethods.PaginationByRequestModel(searchModel).Select(method =>
                {
                    //fill in model values from the entity
                    var externalAuthenticationMethodModel = method.ToPluginModel<ExternalAuthenticationMethodModel>();

                    //fill in additional values (not existing in the entity)
                    externalAuthenticationMethodModel.IsActive = _externalAuthenticationService.IsExternalAuthenticationMethodActive(method);
                    externalAuthenticationMethodModel.ConfigurationUrl = method.GetConfigurationPageUrl();

                    return externalAuthenticationMethodModel;
                }),
                Total = externalAuthenticationMethods.Count
            };

            return model;
        }

        #endregion
    }
}