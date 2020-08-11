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
    public class ContractFormController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractFormService _contractFormService;
        private readonly IContractModelFactory _contractModelFactory;
        #endregion

        #region Ctor

        public ContractFormController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IContractModelFactory contractModelFactory,
            IContractFormService contractFormService)
        {
            this._contractModelFactory = contractModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractFormService = contractFormService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractFormSearchModel(new ContractFormSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ContractFormSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _contractModelFactory.PrepareContractFormListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractFormModel(new ContractFormModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractFormModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractForm>();

                //ensure we have "/" at the end


                _contractFormService.InsertContractForm(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractForm",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractForm"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractForm.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractFormModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult _Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractFormModel(new ContractFormModel(), null);

            return PartialView(model);
        }

        [HttpPost]
        public virtual IActionResult _Create(ContractFormModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ContractForm>();

                //ensure we have "/" at the end


                _contractFormService.InsertContractForm(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractForm",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractForm"), item.Id), item);

                //SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractForm.Added"));
                 return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.ContractForm.AddNew"), item.ToModel<ContractFormModel>());
            }

            //prepare model
            model = _contractModelFactory.PrepareContractFormModel(model, null);

            //if we got this far, something failed, redisplay form
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewContractForm"), model);
        }
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractFormService.GetContractFormById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractModelFactory.PrepareContractFormModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ContractFormModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractFormService.GetContractFormById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _contractFormService.UpdateContractForm(item);

                //activity log
                _customerActivityService.InsertActivity("EditContractForm",
                    string.Format(_localizationService.GetResource("ActivityLog.EditContractForm"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractForm.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _contractModelFactory.PrepareContractFormModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContractForm))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractFormService.GetContractFormById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _contractFormService.DeleteContractForm(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteContractForm",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContractForm"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ContractForm.Deleted"));

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
