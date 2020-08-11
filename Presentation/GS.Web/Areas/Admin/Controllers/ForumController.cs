﻿using System;
using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Forums;
using GS.Services.Forums;
using GS.Services.Localization;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Forums;
using GS.Web.Framework.Mvc.Filters;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class ForumController : BaseAdminController
    {
        #region Fields

        private readonly IForumModelFactory _forumModelFactory;
        private readonly IForumService _forumService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public ForumController(IForumModelFactory forumModelFactory,
            IForumService forumService,
            ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            this._forumModelFactory = forumModelFactory;
            this._forumService = forumService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //prepare model
            var model = _forumModelFactory.PrepareForumGroupSearchModel(new ForumGroupSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ForumGroupList(ForumGroupSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _forumModelFactory.PrepareForumGroupListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult ForumList(ForumSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedKendoGridJson();

            //try to get a forum group with the specified id
            var forumGroup = _forumService.GetForumGroupById(searchModel.ForumGroupId)
                ?? throw new ArgumentException("No forum group found with the specified id");

            //prepare model
            var model = _forumModelFactory.PrepareForumListModel(searchModel, forumGroup);

            return Json(model);
        }

        #endregion

        #region Create

        public virtual IActionResult CreateForumGroup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //prepare model
            var model = _forumModelFactory.PrepareForumGroupModel(new ForumGroupModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CreateForumGroup(ForumGroupModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var forumGroup = model.ToEntity<ForumGroup>();
                forumGroup.CreatedOnUtc = DateTime.UtcNow;
                forumGroup.UpdatedOnUtc = DateTime.UtcNow;
                _forumService.InsertForumGroup(forumGroup);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Added"));

                return continueEditing ? RedirectToAction("EditForumGroup", new { forumGroup.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _forumModelFactory.PrepareForumGroupModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult CreateForum()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //prepare model
            var model = _forumModelFactory.PrepareForumModel(new ForumModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CreateForum(ForumModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var forum = model.ToEntity<Forum>();
                forum.CreatedOnUtc = DateTime.UtcNow;
                forum.UpdatedOnUtc = DateTime.UtcNow;
                _forumService.InsertForum(forum);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.Forum.Added"));

                return continueEditing ? RedirectToAction("EditForum", new { forum.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _forumModelFactory.PrepareForumModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Edit

        public virtual IActionResult EditForumGroup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum group with the specified id
            var forumGroup = _forumService.GetForumGroupById(id);
            if (forumGroup == null)
                return RedirectToAction("List");

            //prepare model
            var model = _forumModelFactory.PrepareForumGroupModel(null, forumGroup);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult EditForumGroup(ForumGroupModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum group with the specified id
            var forumGroup = _forumService.GetForumGroupById(model.Id);
            if (forumGroup == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                forumGroup = model.ToEntity(forumGroup);
                forumGroup.UpdatedOnUtc = DateTime.UtcNow;
                _forumService.UpdateForumGroup(forumGroup);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Updated"));

                return continueEditing ? RedirectToAction("EditForumGroup", forumGroup.Id) : RedirectToAction("List");
            }

            //prepare model
            model = _forumModelFactory.PrepareForumGroupModel(model, forumGroup, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult EditForum(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum with the specified id
            var forum = _forumService.GetForumById(id);
            if (forum == null)
                return RedirectToAction("List");

            //prepare model
            var model = _forumModelFactory.PrepareForumModel(null, forum);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult EditForum(ForumModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum with the specified id
            var forum = _forumService.GetForumById(model.Id);
            if (forum == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                forum = model.ToEntity(forum);
                forum.UpdatedOnUtc = DateTime.UtcNow;
                _forumService.UpdateForum(forum);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.Forum.Updated"));

                return continueEditing ? RedirectToAction("EditForum", forum.Id) : RedirectToAction("List");
            }

            //prepare model
            model = _forumModelFactory.PrepareForumModel(model, forum, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public virtual IActionResult DeleteForumGroup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum group with the specified id
            var forumGroup = _forumService.GetForumGroupById(id);
            if (forumGroup == null)
                return RedirectToAction("List");

            _forumService.DeleteForumGroup(forumGroup);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteForum(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageForums))
                return AccessDeniedView();

            //try to get a forum with the specified id
            var forum = _forumService.GetForumById(id);
            if (forum == null)
                return RedirectToAction("List");

            _forumService.DeleteForum(forum);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.Forums.Forum.Deleted"));

            return RedirectToAction("List");
        }

        #endregion

        #endregion
    }
}