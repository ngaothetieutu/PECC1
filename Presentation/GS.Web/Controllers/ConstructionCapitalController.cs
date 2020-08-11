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
    public class ConstructionCapitalController : BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IConstructionCapitalService _constructionCapitalService;
        private readonly IConstructionModelFactory _constructionModelFactory;
        #endregion

        #region Ctor

        public ConstructionCapitalController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IConstructionModelFactory constructionModelFactory,
            IConstructionCapitalService constructionCapitalService)
        {
            this._constructionModelFactory = constructionModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._constructionCapitalService = constructionCapitalService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionCapitalSearchModel(new ConstructionCapitalSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ConstructionCapitalSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionCapitalListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionCapitalModel(new ConstructionCapitalModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ConstructionCapitalModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ConstructionCapital>();

                //ensure we have "/" at the end


                _constructionCapitalService.InsertConstructionCapital(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewConstructionCapital",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewConstructionCapital"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionCapital.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionCapitalModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionCapitalService.GetConstructionCapitalById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionCapitalModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ConstructionCapitalModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionCapitalService.GetConstructionCapitalById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _constructionCapitalService.UpdateConstructionCapital(item);

                //activity log
                _customerActivityService.InsertActivity("EditConstructionCapital",
                    string.Format(_localizationService.GetResource("ActivityLog.EditConstructionCapital"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionCapital.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionCapitalModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionCapital))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionCapitalService.GetConstructionCapitalById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _constructionCapitalService.DeleteConstructionCapital(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteConstructionCapital",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteConstructionCapital"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionCapital.Deleted"));

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
