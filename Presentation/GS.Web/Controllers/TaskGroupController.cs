using System;
using Microsoft.AspNetCore.Mvc;
using GS.Services.Configuration;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;
using GS.Web.Factories;
using GS.Web.Models.Works;
using GS.Core.Domain.Works;
using GS.Services.Works;

namespace GS.Web.Controllers
{
    public class TaskGroupController: BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly ITaskGroupService _taskGroupService;
        private readonly ITaskModelFactory _taskModelFactory;
        #endregion

        #region Ctor

        public TaskGroupController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            ITaskModelFactory taskModelFactory,
            ITaskGroupService taskGroupService)
        {
            this._taskModelFactory = taskModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._taskGroupService = taskGroupService;
        }

        #endregion
        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            //prepare model
            var model = _taskModelFactory.PrepareTaskGroupSearchModel(new TaskGroupSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(TaskGroupSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedKendoGridJson();
            //prepare model
            var model = _taskModelFactory.PrepareTaskGroupListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            //prepare model
            var model = _taskModelFactory.PrepareTaskGroupModel(new TaskGroupModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(TaskGroupModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = model.ToEntity<TaskGroup>();

                //ensure we have "/" at the end


                _taskGroupService.InsertTaskGroup(item);

                //activity log
                _customerActivityService.InsertActivity("AddNewTaskGroup",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewTaskGroup"), item.Id), item);

                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.TaskGroup.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _taskModelFactory.PrepareTaskGroupModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _taskGroupService.GetTaskGroupById(id);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _taskModelFactory.PrepareTaskGroupModel(null, item);

            return View(model);
        }
        public virtual IActionResult _getTaskGroupvalue(int id)
        {            
            var item = _taskGroupService.GetTaskGroupById(id); 
            if(item==null)
            {
                return JsonErrorMessage("Error",item);
            }
            return JsonSuccessMessage("Ok",item);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(TaskGroupModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _taskGroupService.GetTaskGroupById(model.Id);
            if (item == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                item = model.ToEntity(item);


                _taskGroupService.UpdateTaskGroup(item);

                //activity log
                _customerActivityService.InsertActivity("EditTaskGroup",
                    string.Format(_localizationService.GetResource("ActivityLog.EditTaskGroup"), item.Id), item);



                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.TaskGroup.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _taskModelFactory.PrepareTaskGroupModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTaskGroup))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _taskGroupService.GetTaskGroupById(id);
            if (item == null)
                return RedirectToAction("List");

            try
            {
                _taskGroupService.DeleteTaskGroup(item);

                //activity log
                _customerActivityService.InsertActivity("DeleteTaskGroup",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteTaskGroup"), item.Id), item);


                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.TaskGroup.Deleted"));

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
