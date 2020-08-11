using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class HomepageCategoriesViewComponent : GSViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public HomepageCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _catalogModelFactory.PrepareHomepageCategoryModels();
            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
