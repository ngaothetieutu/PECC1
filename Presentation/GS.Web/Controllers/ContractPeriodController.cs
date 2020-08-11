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
    public class ContractPeriodController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractPeriodService _contractPeriodService;
        private readonly IContractModelFactory _contractModelFactory;
        #endregion

        #region Ctor

        public ContractPeriodController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IContractModelFactory contractModelFactory,
            IContractPeriodService ContractPeriodService)
        {
            this._contractModelFactory = contractModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractPeriodService = ContractPeriodService;
        }

        #endregion
        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractPeriodSearchModel(new ContractPeriodSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ContractPeriodSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _contractModelFactory.PrepareContractPeriodListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractPeriodModel(new ContractPeriodModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractPeriodModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractPeriod>();

                //ensure we have "/" at the end


                _contractPeriodService.InsertContractPeriod(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractPeriod",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractPeriod"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractPeriod.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractPeriodModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult _Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractPeriodModel(new ContractPeriodModel(), null);

            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _Create(ContractPeriodModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractPeriod>();

                //ensure we have "/" at the end


                _contractPeriodService.InsertContractPeriod(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractPeriod",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractPeriod"), item.Id), item);

                //SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractPeriod.Added"));

                return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.ContractPeriod.AddNew"), item.ToModel<ContractPeriodModel>());
            }

            //prepare model
            model = _contractModelFactory.PrepareContractPeriodModel(model, null);

            //if we got this far, something failed, redisplay form
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.Contract.ContractPeriod.AddNew"), model);
        }
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractPeriodService.GetContractPeriodById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractModelFactory.PrepareContractPeriodModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ContractPeriodModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractPeriodService.GetContractPeriodById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _contractPeriodService.UpdateContractPeriod(item);

                //activity log
                _customerActivityService.InsertActivity("EditContractPeriod",
                    string.Format(_localizationService.GetResource("ActivityLog.EditContractPeriod"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractPeriod.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractPeriodModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractPeriod))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractPeriodService.GetContractPeriodById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _contractPeriodService.DeleteContractPeriod(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteContractPeriod",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContractPeriod"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractPeriod.Deleted"));

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
