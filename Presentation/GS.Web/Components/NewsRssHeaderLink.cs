using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.News;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class NewsRssHeaderLinkViewComponent : GSViewComponent
    {
        private readonly NewsSettings _newsSettings;

        public NewsRssHeaderLinkViewComponent(NewsSettings newsSettings)
        {
            this._newsSettings = newsSettings;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowHeaderRssUrl)
                return Content("");

            return View();
        }
    }
}
