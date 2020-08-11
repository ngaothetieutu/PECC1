using System;
using Microsoft.AspNetCore.Mvc;
using GS.Core;
using GS.Core.Domain.Customers;
using GS.Services.Catalog;
using GS.Services.Customers;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class CustomerRoleController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerRoleModelFactory _customerRoleModelFactory;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CustomerRoleController(ICustomerActivityService customerActivityService,
            ICustomerRoleModelFactory customerRoleModelFactory,
            ICustomerService customerService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            this._customerActivityService = customerActivityService;
            this._customerRoleModelFactory = customerRoleModelFactory;
            this._customerService = customerService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = _customerRoleModelFactory.PrepareCustomerRoleSearchModel(new CustomerRoleSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(CustomerRoleSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _customerRoleModelFactory.PrepareCustomerRoleListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = _customerRoleModelFactory.PrepareCustomerRoleModel(new CustomerRoleModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CustomerRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var customerRole = model.ToEntity<CustomerRole>();
                _customerService.InsertCustomerRole(customerRole);

                //activity log
                _customerActivityService.InsertActivity("AddNewCustomerRole",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name), customerRole);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _customerRoleModelFactory.PrepareCustomerRoleModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                return RedirectToAction("List");

            //prepare model
            var model = _customerRoleModelFactory.PrepareCustomerRoleModel(null, customerRole);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CustomerRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var customerRole = _customerService.GetCustomerRoleById(model.Id);
            if (customerRole == null)
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (customerRole.IsSystemRole && !model.Active)
                        throw new GSException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                    if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new GSException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                    if (GSCustomerDefaults.RegisteredRoleName.Equals(customerRole.SystemName, StringComparison.InvariantCultureIgnoreCase) &&
                        model.PurchasedWithProductId > 0)
                        throw new GSException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.PurchasedWithProduct.Registered"));

                    customerRole = model.ToEntity(customerRole);
                    _customerService.UpdateCustomerRole(customerRole);

                    //activity log
                    _customerActivityService.InsertActivity("EditCustomerRole",
                        string.Format(_localizationService.GetResource("ActivityLog.EditCustomerRole"), customerRole.Name), customerRole);

                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Updated"));

                    return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _customerRoleModelFactory.PrepareCustomerRoleModel(model, customerRole, true);

                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                return RedirectToAction("List");

            try
            {
                _customerService.DeleteCustomerRole(customerRole);

                //activity log
                _customerActivityService.InsertActivity("DeleteCustomerRole",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteCustomerRole"), customerRole.Name), customerRole);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        

        #endregion
    }
}