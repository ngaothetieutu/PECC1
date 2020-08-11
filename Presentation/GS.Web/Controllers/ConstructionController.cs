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
    public class ConstructionController : BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IConstructionService _constructionService;
        private readonly IConstructionModelFactory _constructionModelFactory;
        #endregion

        #region Ctor

        public ConstructionController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IConstructionModelFactory constructionModelFactory,
            IConstructionService constructionService)
        {
            this._constructionModelFactory = constructionModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._constructionService = constructionService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionSearchModel(new ConstructionSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ConstructionSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionListModel(searchModel);
            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionModel(new ConstructionModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ConstructionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<Construction>();

                //ensure we have "/" at the end
                _constructionService.InsertConstruction(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewConstruction",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewConstruction"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Construction.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult _Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionModel(new ConstructionModel(), null);

            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _Create(ConstructionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<Construction>();

                //ensure we have "/" at the end
                _constructionService.InsertConstruction(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewConstruction",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewConstruction"), item.Id), item);

                //SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Construction.Added"));
                return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewConstruction"), item.ToModel<ConstructionModel>());
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionModel(model, null);

            //if we got this far, something failed, redisplay form
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewConstruction"), model);
        }
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionService.GetConstructionById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ConstructionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionService.GetConstructionById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                _constructionModelFactory.PrepareConstruction(model, item);
                _constructionService.UpdateConstruction(item);
                //activity log
                _customerActivityService.InsertActivity("EditConstruction",
                    string.Format(_localizationService.GetResource("ActivityLog.EditConstruction"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Construction.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionService.GetConstructionById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _constructionService.DeleteConstruction(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteConstruction",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteConstruction"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Construction.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = item.Id });
            }
        }

        //public virtual IActionResult GetListConstruction(ConstructionSearchModel searchModel)
        //{
        //    var listConstruction = _constructionService.GetContructionByName(pageSize: 4);
        //    return RedirectToAction("List");
        //}
       
        #endregion
    }
}
