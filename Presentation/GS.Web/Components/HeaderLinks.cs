using Microsoft.AspNetCore.Mvc;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class HeaderLinksViewComponent : GSViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public HeaderLinksViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareHeaderLinksModel();
            return View(model);
        }
    }
}
