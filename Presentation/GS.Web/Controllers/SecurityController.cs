using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using GS.Core;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Security;
using GS.Services.Customers;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Models.Security;
using GS.Web.Controllers;

namespace GS.Web.Controllers
{
    public partial class SecurityController : BaseWorksController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IPermissionService _permissionService;
        private readonly ISecurityModelFactory _securityModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public SecurityController(ICustomerService customerService,
            ILocalizationService localizationService,
            ILogger logger,
            IPermissionService permissionService,
            ISecurityModelFactory securityModelFactory,
            IWorkContext workContext)
        {
            this._customerService = customerService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._permissionService = permissionService;
            this._securityModelFactory = securityModelFactory;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult AccessDenied(string pageUrl)
        {
            var currentCustomer = _workContext.CurrentCustomer;
            if (currentCustomer == null || currentCustomer.IsGuest())
            {
                _logger.Information($"Access denied to anonymous request on {pageUrl}");
                return View();
            }

            _logger.Information($"Access denied to user #{currentCustomer.Email} '{currentCustomer.Email}' on {pageUrl}");

            return View();
        }
        #endregion
    }
}