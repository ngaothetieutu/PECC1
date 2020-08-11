using System;
using Microsoft.AspNetCore.Mvc;
using GS.Services.Configuration;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;
using GS.Web.Areas.Admin.Factories;
using GS.Core.Domain.Customers;
using GS.Services.Customers;
using System.Collections.Generic;
using GS.Web.Models.Unit;
using GS.Web.Factories;

namespace GS.Web.Controllers
{
    public class UnitController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;

        private readonly IUnitService _unitService;
        private readonly IUnitModelFactory _unitModelFactory;
        #endregion

        #region Ctor

        public UnitController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IUnitModelFactory unitModelFactory,
            IUnitService unitService)
        {
            this._unitModelFactory = unitModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._unitService = unitService;
        }

        #endregion
        #region Unit
        
        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();

            //prepare model
            var model = _unitModelFactory.PrepareUnitSearchModel(new UnitSearchModel());

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult List(UnitSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _unitModelFactory.PrepareUnitListModel(searchModel);

            return Json(model);
        }
        [HttpPost]
        public virtual IActionResult TreeList(UnitSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _unitModelFactory.PrepareUnitTreeListModel(searchModel);
            return Json(model);
        }

       

        public virtual IActionResult Edit(int? id, int? paren_id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();
            UnitModel model = new UnitModel();
            if (id != null)
            {
                var item = _unitService.GetUnitById((int)id);
                //prepare model
                model = _unitModelFactory.PrepareUnitModel(null, item);
            }
            else if (paren_id != null)
            {

                var item = _unitService.GetUnitById((int)paren_id);
                //prepare model
                model.ParentId = (int)paren_id;
                model.ParentName = item.Name;
                model = _unitModelFactory.PrepareUnitModel(model, null);
            }
            else
            {
                //prepare model
                model = _unitModelFactory.PrepareUnitModel(model, null);
            }
            return PartialView(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(UnitModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _unitService.GetUnitById(model.Id);
            try {                
                //kiểm tra là cập nhật hay thêm mới
                if (item != null)
                {
                    if (ModelState.IsValid)
                    {
                        item = model.ToEntity(item);
                        var chk = _unitService.UpdateUnit(item);
                        if (!chk)
                        {
                            ErrorNotification(_localizationService.GetResource("AppWork.Customer.Unit.UnitCodeExist"));
                            return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
                        }

                        //activity log
                        _customerActivityService.InsertActivity("EditUnit",
                        string.Format(_localizationService.GetResource("ActivityLog.EditUnit"), item.Id), item);

                        SuccessNotification(_localizationService.GetResource("AppWork.Customer.Unit.Updated"));
                        return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        item = model.ToEntity<Unit>();
                        var chk = _unitService.InsertUnit(item);
                        if (!chk)
                        {
                            ErrorNotification(_localizationService.GetResource("AppWork.Customer.Unit.UnitCodeExist"));
                            return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
                        }

                        //activity log
                        _customerActivityService.InsertActivity("AddNewUnit",
                            string.Format(_localizationService.GetResource("ActivityLog.AddNewUnit"), item.Id), item);

                        SuccessNotification(_localizationService.GetResource("AppWork.Customer.Unit.Added"));
                        return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
                    }
                }
                ErrorNotification(_localizationService.GetResource("AppWork.Customer.Unit.UnitCodeExist"));
                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }
            catch
            {
                ErrorNotification(_localizationService.GetResource("AppWork.Customer.Unit.ErrorNotification"));
                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _unitService.GetUnitById(id);
            if (item == null)
                return RedirectToAction("List");
            try
            {
                var chk = _unitService.DeleteUnit(item);
                if (!chk)
                {
                    SuccessNotification(_localizationService.GetResource("AppWork.Customer.Unit.NotDeleted"));
                    return RedirectToAction("List");
                }
                //activity log
                _customerActivityService.InsertActivity("DeleteUnit",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteUnit"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Unit.Deleted"));
                return RedirectToAction("List");
            }
            catch
            {
                SuccessNotification(_localizationService.GetResource("AppWork.Customer.Unit.ErrorNotification"));
                return RedirectToAction("List");
            }
        }
        #endregion
        #region  Nguoi dung
        public virtual ActionResult _Customer(int UnitId)
        {
            var model = new CustomerUnitSearchModel();
            model.UnitId = UnitId;
            return PartialView(model);
        }
        
        [HttpPost]
        public virtual IActionResult CustomerUnit(CustomerUnitSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView(); 
            var model = _unitModelFactory.PrepareCustomerUnitListModel(searchModel);
            return Json(model);
        }
        public virtual ActionResult _AddCustomer(CustomerUnitSearchModel model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _SearchCustomer(CustomerUnitSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();
            var model = _unitModelFactory.PrepareCustomerSearchListModel(searchModel);
            return PartialView("_SearchListCustomer", model);
        }
        public virtual IActionResult _SelectListCustomer(CustomerUnitSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();
            var model = _unitModelFactory.PrepareCustomerCheckListModel(searchModel);
            return PartialView("_SelectCustomerList", model);
        }
        [HttpPost]
        public virtual IActionResult AddCustomer(CustomerUnitModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();

            var item = _unitService.GetUnitById(model.UnitId);
            var chk = _unitService.insertCustomerUnit(model.UnitId, model.CustomerId);
            if (chk)
            {
                //activity log
                _customerActivityService.InsertActivity("AddNewCustomerUnit",
                        string.Format(_localizationService.GetResource("ActivityLog.AddNewCustomerUnit"), item.Id), item);

                return this.JsonSuccessMessage(_localizationService.GetResource("AppWork.Customer.CustomerUnit.Added"));
            }
            else
            {
                //activity log
                _customerActivityService.InsertActivity("AddNewCustomerUnit",
                        string.Format(_localizationService.GetResource("ActivityLog.AddNewCustomerUnit"), item.Id), item);

                return this.JsonErrorMessage(_localizationService.GetResource("AppWork.Customer.CustomerUnit.NotAdded"));
            }            
        }
        
        [HttpPost]
        public virtual IActionResult DeleteCustomerUnit(CustomerUnitModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUnit))
                return AccessDeniedView();
            var item = _unitService.GetUnitById(model.UnitId);
            var chk = _unitService.deleteCustomerUnit(model.UnitId, model.CustomerId);
            if (chk)
            {
                //activity log
                _customerActivityService.InsertActivity("DeleteCustomerUnit",
                        string.Format(_localizationService.GetResource("ActivityLog.DeleteCustomerUnit"), item.Id), item);
                return this.JsonSuccessMessage(_localizationService.GetResource("AppWork.Customer.CustomerUnit.Deleted"));
            }
            else
            {
                //activity log
                _customerActivityService.InsertActivity("DeleteCustomerUnit",
                        string.Format(_localizationService.GetResource("ActivityLog.DeleteCustomerUnit"), item.Id), item);
                return this.JsonErrorMessage(_localizationService.GetResource("AppWork.Customer.CustomerUnit.NotDeleted"));
            }            
        }
        #endregion





    }
}
