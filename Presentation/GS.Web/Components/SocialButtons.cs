using Microsoft.AspNetCore.Mvc;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class SocialButtonsViewComponent : GSViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public SocialButtonsViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareSocialModel();
            return View(model);
        }
    }
}
