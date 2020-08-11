using Microsoft.AspNetCore.Mvc;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class CurrencySelectorViewComponent : GSViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public CurrencySelectorViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareCurrencySelectorModel();
            if (model.AvailableCurrencies.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
