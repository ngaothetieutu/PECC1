﻿using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Blogs;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class BlogTagsViewComponent : GSViewComponent
    {
        private readonly BlogSettings _blogSettings;
        private readonly IBlogModelFactory _blogModelFactory;

        public BlogTagsViewComponent(BlogSettings blogSettings, IBlogModelFactory blogModelFactory)
        {
            this._blogSettings = blogSettings;
            this._blogModelFactory = blogModelFactory;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = _blogModelFactory.PrepareBlogPostTagListModel();
            return View(model);
        }
    }
}