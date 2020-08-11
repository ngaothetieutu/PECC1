using System;
using Microsoft.AspNetCore.Mvc;
using GS.Services.Configuration;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;
using GS.Services.Contracts;
using GS.Web.Factories;
using GS.Web.Models.Contracts;
using GS.Core.Domain.Contracts;

namespace GS.Web.Controllers
{
    public class ContractTypeController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IContractModelFactory _contractModelFactory;
        #endregion

        #region Ctor

        public ContractTypeController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IContractModelFactory contractModelFactory,
            IContractTypeService contractTypeService)
        {
            this._contractModelFactory = contractModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractTypeService = contractTypeService;
        }

        #endregion
        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractTypeSearchModel(new ContractTypeSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ContractTypeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _contractModelFactory.PrepareContractTypeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractTypeModel(new ContractTypeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractType>();

                //ensure we have "/" at the end


                _contractTypeService.InsertContractType(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractType",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractType"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractType.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractTypeModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult _Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractTypeModel(new ContractTypeModel(), null);

            return PartialView(model);
        }

        [HttpPost]
        public virtual IActionResult _Create(ContractTypeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractType>();

                //ensure we have "/" at the end


                _contractTypeService.InsertContractType(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractType",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractType"), item.Id), item);
                return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.ContractType.Added"), item.ToModel<ContractTypeModel>());
            }

            //prepare model
            model = _contractModelFactory.PrepareContractTypeModel(model, null);

            //if we got this far, something failed, redisplay form
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewContractTypeError"),model);
        }
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractTypeService.GetContractTypeById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractModelFactory.PrepareContractTypeModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ContractTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractTypeService.GetContractTypeById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _contractTypeService.UpdateContractType(item);

                //activity log
                _customerActivityService.InsertActivity("EditContractType",
                    string.Format(_localizationService.GetResource("ActivityLog.EditContractType"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractType.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractTypeModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractTypeService.GetContractTypeById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _contractTypeService.DeleteContractType(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteContractType",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContractType"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractType.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = item.Id });
            }
        }

        #endregion
    }
}
