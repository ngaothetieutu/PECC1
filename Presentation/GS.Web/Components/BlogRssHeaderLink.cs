using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Blogs;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class BlogRssHeaderLinkViewComponent : GSViewComponent
    {
        private readonly BlogSettings _blogSettings;

        public BlogRssHeaderLinkViewComponent(BlogSettings blogSettings)
        {
            this._blogSettings = blogSettings;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled || !_blogSettings.ShowHeaderRssUrl)
                return Content("");

            return View();
        }
    }
}
