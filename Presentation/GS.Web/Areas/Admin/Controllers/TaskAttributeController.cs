using System;
using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Tasks;
using GS.Services.Tasks;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Tasks;
using GS.Web.Framework.Mvc;
using GS.Web.Framework.Mvc.Filters;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class TaskAttributeController : BaseAdminController
    {
        #region Fields
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ITaskAttributeModelFactory _taskAttributeModelFactory;
        private readonly ITaskAttributeService _taskAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        #endregion

        #region Ctor
        public TaskAttributeController(ICustomerActivityService customerActivityService,
            ITaskAttributeModelFactory taskAttributeModelFactory,
            ITaskAttributeService taskAttributeService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService)
        {
            this._customerActivityService = customerActivityService;
            this._taskAttributeModelFactory = taskAttributeModelFactory;
            this._taskAttributeService = taskAttributeService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
        }
        #endregion

        #region Utilities
        protected virtual void UpdateAttributeLocales(TaskAttribute taskAttribute, TaskAttributeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(taskAttribute,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdateValueLocales(TaskAttributeValue taskAttributeValue, TaskAttributeValueModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(taskAttributeValue,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Task Attribute
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //select "customer form fields" tab
            SaveSelectedTabName("tab-taskformfields");

            //we just redirect a user to the customer settings page
            return RedirectToAction("TaskAttribute", "Setting");
        }

        [HttpPost]
        public virtual IActionResult List(TaskAttributeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _taskAttributeModelFactory.PrepareTaskAttributeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _taskAttributeModelFactory.PrepareTaskAttributeModel(new TaskAttributeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(TaskAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var taskAttribute = model.ToEntity<TaskAttribute>();
                _taskAttributeService.InsertTaskAttribute(taskAttribute);

                //activity log
                _customerActivityService.InsertActivity("AddNewTaskAttribute",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewTaskAttribute"), taskAttribute.Id),
                    taskAttribute);

                //locales
                UpdateAttributeLocales(taskAttribute, model);

                SuccessNotification(_localizationService.GetResource("Admin.Task.TaskAttributes.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = taskAttribute.Id });
            }

            //prepare model
            model = _taskAttributeModelFactory.PrepareTaskAttributeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(id);
            if (taskAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _taskAttributeModelFactory.PrepareTaskAttributeModel(null, taskAttribute);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(TaskAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var taskAttribute = _taskAttributeService.GetTaskAttributeById(model.Id);
            if (taskAttribute == null)
                //no customer attribute found with the specified id
                return RedirectToAction("List");

            if (!ModelState.IsValid)
                //if we got this far, something failed, redisplay form
                return View(model);

            taskAttribute = model.ToEntity(taskAttribute);
            _taskAttributeService.UpdateTaskAttribute(taskAttribute);

            //activity log
            _customerActivityService.InsertActivity("EditTaskAttribute",
                string.Format(_localizationService.GetResource("ActivityLog.EditTaskAttribute"), taskAttribute.Id),
                taskAttribute);

            //locales
            UpdateAttributeLocales(taskAttribute, model);

            SuccessNotification(_localizationService.GetResource("Admin.Task.TaskAttributes.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            //selected tab
            SaveSelectedTabName();

            return RedirectToAction("Edit", new { id = taskAttribute.Id });
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var taskAttribute = _taskAttributeService.GetTaskAttributeById(id);
            _taskAttributeService.DeleteTaskAttribute(taskAttribute);

            //activity log
            _customerActivityService.InsertActivity("DeleteTaskAttribute",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteTaskAttribute"), taskAttribute.Id),
                taskAttribute);

            SuccessNotification(_localizationService.GetResource("Admin.Task.TaskAttributes.Deleted"));
            return RedirectToAction("List");
        }
        #endregion

        #region Task attribute value
        [HttpPost]
        public virtual IActionResult ValueList(TaskAttributeValueSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //try to get a customer attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(searchModel.TaskAttributeId)
                ?? throw new ArgumentException("No task attribute found with the specified id");

            //prepare model
            var model = _taskAttributeModelFactory.PrepareTaskAttributeValueListModel(searchModel, taskAttribute);

            return Json(model);
        }

        public virtual IActionResult ValueCreatePopup(int taskAttributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(taskAttributeId);
            if (taskAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _taskAttributeModelFactory
                .PrepareTaskAttributeValueModel(new TaskAttributeValueModel(), taskAttribute, null);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueCreatePopup(TaskAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(model.TaskAttributeId);
            if (taskAttribute == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var cav = model.ToEntity<TaskAttributeValue>();
                _taskAttributeService.InsertTaskAttributeValue(cav);

                //activity log
                _customerActivityService.InsertActivity("AddNewTaskAttributeValue",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewTaskAttributeValue"), cav.Id), cav);

                UpdateValueLocales(cav, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _taskAttributeModelFactory.PrepareTaskAttributeValueModel(model, taskAttribute, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult ValueEditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a task attribute value with the specified id
            var taskAttributeValue = _taskAttributeService.GetTaskAttributeValueById(id);
            if (taskAttributeValue == null)
                return RedirectToAction("List");

            //try to get a task attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(taskAttributeValue.TaskAttributeId);
            if (taskAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _taskAttributeModelFactory.PrepareTaskAttributeValueModel(null, taskAttribute, taskAttributeValue);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueEditPopup(TaskAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute value with the specified id
            var taskAttributeValue = _taskAttributeService.GetTaskAttributeValueById(model.Id);
            if (taskAttributeValue == null)
                return RedirectToAction("List");

            //try to get a customer attribute with the specified id
            var taskAttribute = _taskAttributeService.GetTaskAttributeById(taskAttributeValue.TaskAttributeId);
            if (taskAttribute == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                taskAttributeValue = model.ToEntity(taskAttributeValue);
                _taskAttributeService.UpdateTaskAttributeValue(taskAttributeValue);

                //activity log
                _customerActivityService.InsertActivity("EditTaskAttributeValue",
                    string.Format(_localizationService.GetResource("ActivityLog.EditTaskAttributeValue"), taskAttributeValue.Id),
                    taskAttributeValue);

                UpdateValueLocales(taskAttributeValue, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _taskAttributeModelFactory.PrepareTaskAttributeValueModel(model, taskAttribute, taskAttributeValue, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute value with the specified id
            var taskAttributeValue = _taskAttributeService.GetTaskAttributeValueById(id)
                ?? throw new ArgumentException("No task attribute value found with the specified id", nameof(id));

            _taskAttributeService.DeleteTaskAttributeValue(taskAttributeValue);

            //activity log
            _customerActivityService.InsertActivity("DeleteTaskAttributeValue",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteTaskAttributeValue"), taskAttributeValue.Id),
                taskAttributeValue);

            return new NullJsonResult();
        }
        #endregion
    }
}
