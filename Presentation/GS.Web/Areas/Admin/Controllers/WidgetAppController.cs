using GS.Core.Domain.WidgetApps;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Services.WidgetApps;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.WidgetApps;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class WidgetAppController : BaseAdminController
    {
        #region Fields
        private readonly IPermissionService _permissionService;
        private readonly IWidgetAppService _widgetAppService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly IWidgetAppModelFactory _widgetAppModelFactory;
        #endregion
        #region Ctor
        public WidgetAppController(IPermissionService permissionService,
            IWidgetAppService widgetAppService,
            ILocalizationService localizationService,
            IWidgetAppModelFactory widgetAppModelFactory,
            ICustomerActivityService customerActivityService)
        {
            this._widgetAppModelFactory = widgetAppModelFactory;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._widgetAppService = widgetAppService;
            this._localizationService = localizationService;
        }
        #endregion

        #region WidgetApp
        public virtual IActionResult Index()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var model = new WidgetAppModel();
            return View(model);
        }
        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var model = new WidgetAppSearchModel();
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult List(WidgetAppSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var item = _widgetAppModelFactory.PrepareWidgetAppListModel(model);
            return Json(item);
        }
        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var model = new WidgetAppModel();
            model = _widgetAppModelFactory.PrepareWidgetAppModel(model, null);

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Create(WidgetAppModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            
                var widgetApp = model.ToEntity<WidgetApp>();
                _widgetAppService.InsertWidgetApp(widgetApp);

                _customerActivityService.InsertActivity("AddNewWidgetApp",
                       string.Format(_localizationService.GetResource("ActivityLog.AddNewWidgetApp"), widgetApp.Id),
                       widgetApp);

                SuccessNotification(_localizationService.GetResource("Admin.WidgetApps.WidgetApp.Added"));

            model = _widgetAppModelFactory.PrepareWidgetAppModel(model, null);

            return View(model);
        }
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var widgetApp = _widgetAppService.GetWidgetAppById(id);
            if (widgetApp == null)
                return RedirectToAction("List");
            var model = _widgetAppModelFactory.PrepareWidgetAppModel(null, widgetApp);
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Edit (WidgetAppModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            var widgetApp = _widgetAppService.GetWidgetAppById(model.Id);
            if (widgetApp == null)
                return RedirectToAction("List");
            widgetApp = model.ToEntity(widgetApp);
            _widgetAppService.UpdateWidgetApp(widgetApp);

            return RedirectToAction("Edit", new { id = widgetApp.Id });
        }
        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var widgetApp = _widgetAppService.GetWidgetAppById(id);
            _widgetAppService.DeleteWidgetApp(widgetApp);

            _customerActivityService.InsertActivity("DeleteWidgetApp",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteWidgetApp"), widgetApp.Id),
                widgetApp);

            SuccessNotification(_localizationService.GetResource("Admin.WidgetApps.WidgetApp.Deleted"));
            return RedirectToAction("List");
        }
        #endregion
    }
}
