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
    public partial class ConstructionTypeController : BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IConstructionTypeService _constructionTypeService;
        private readonly IConstructionModelFactory _constructionModelFactory;




        #endregion

        #region Ctor

        public ConstructionTypeController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IConstructionModelFactory constructionModelFactory,
            IConstructionTypeService constructionTypeService)
        {
            this._constructionModelFactory = constructionModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._constructionTypeService = constructionTypeService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();
            
            //prepare model
            var model = _constructionModelFactory.PrepareConstructionTypeSearchModel(new ConstructionTypeSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ConstructionTypeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionTypeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionTypeModel(new ConstructionTypeModel(),null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ConstructionTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ConstructionType>();

                //ensure we have "/" at the end
                _constructionTypeService.InsertConstructionType(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewConstructionType",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewConstructionType"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionType.Added"));
                
                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionTypeModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionTypeService.GetConstructionTypeById(id);
            
            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _constructionModelFactory.PrepareConstructionTypeModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ConstructionTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionTypeService.GetConstructionTypeById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _constructionTypeService.UpdateConstructionType(item);

                //activity log
                _customerActivityService.InsertActivity("EditConstructionType",
                    string.Format(_localizationService.GetResource("ActivityLog.EditConstructionType"), item.Id), item);

             

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionType.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _constructionModelFactory.PrepareConstructionTypeModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstructionType))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _constructionTypeService.GetConstructionTypeById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _constructionTypeService.DeleteConstructionType(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteConstructionType",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteConstructionType"), item.Id), item);

           
                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ConstructionType.Deleted"));

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