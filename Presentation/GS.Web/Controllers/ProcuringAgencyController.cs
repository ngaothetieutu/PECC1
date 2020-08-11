using GS.Core.Domain.Contracts;
using GS.Services.Configuration;
using GS.Services.Contracts;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Factories;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;
using GS.Web.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class ProcuringAgencyController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IProcuringAgencyModelFactory _procuringAgencyModelFactory;
        #endregion

        #region Ctor

        public ProcuringAgencyController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IProcuringAgencyModelFactory procuringAgencyModelFactory,
            IProcuringAgencyService procuringAgencyService)
        {
            this._procuringAgencyModelFactory = procuringAgencyModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._procuringAgencyService = procuringAgencyService;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            //prepare model
            var model =_procuringAgencyModelFactory.PrepareProcuringAgencySearchModel(new ProcuringAgencySearchModel());

            return View(model);
        }
        
        [HttpPost]
        public virtual IActionResult List(ProcuringAgencySearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model =_procuringAgencyModelFactory.PrepareProcuringAgencyListModel(searchModel);
            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();
            //prepare model
            var model =_procuringAgencyModelFactory.PrepareProcuringAgencyModel(new ProcuringAgencyModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ProcuringAgencyModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ProcuringAgency>();

                //ensure we have "/" at the end
               _procuringAgencyService.InsertProcuringAgency(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewProcuringAgency",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewProcuringAgency"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ProcuringAgency.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model =_procuringAgencyModelFactory.PrepareProcuringAgencyModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult _Create(ProcuringAgencyModel model, bool continueEditing)
        {         

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<ProcuringAgency>();

                //ensure we have "/" at the end
                _procuringAgencyService.InsertProcuringAgency(item);
                //activity log
                _customerActivityService.InsertActivity("AddNewProcuringAgency",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewProcuringAgency"), item.Id), item);                

                return JsonSuccessMessage("ok", item);
            }    
            //if we got this far, something failed, redisplay form
            return JsonErrorMessage();
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item =_procuringAgencyService.GetProcuringAgencyById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model =_procuringAgencyModelFactory.PrepareProcuringAgencyModel(null, item);

            return View(model);
        }
        public virtual IActionResult _Detail(int id)
        {
          
            //try to get a store with the specified id
            var item = _procuringAgencyService.GetProcuringAgencyById(id);

            if (item == null)
                return PartialView();

            //prepare model
            var model = _procuringAgencyModelFactory.PrepareProcuringAgencyModel(null, item);

            return PartialView(model);
        }
        public virtual IActionResult Detail(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _procuringAgencyService.GetProcuringAgencyById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _procuringAgencyModelFactory.PrepareProcuringAgencyModel(null, item);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ProcuringAgencyModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item =_procuringAgencyService.GetProcuringAgencyById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


               _procuringAgencyService.UpdateProcuringAgency(item);

                //activity log
                _customerActivityService.InsertActivity("EditProcuringAgency",
                    string.Format(_localizationService.GetResource("ActivityLog.EditProcuringAgency"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ProcuringAgency.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model =_procuringAgencyModelFactory.PrepareProcuringAgencyModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProcuringAgency))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item =_procuringAgencyService.GetProcuringAgencyById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
               _procuringAgencyService.DeleteProcuringAgency(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteProcuringAgency",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteProcuringAgency"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.ProcuringAgency.Deleted"));

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
