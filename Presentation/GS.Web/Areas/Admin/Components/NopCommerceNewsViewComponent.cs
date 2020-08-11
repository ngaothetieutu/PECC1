using Microsoft.AspNetCore.Mvc;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays the nopCommerce news
    /// </summary>
    public class GSCommerceNewsViewComponent : GSViewComponent
    {
        #region Fields

        private readonly IHomeModelFactory _homeModelFactory;

        #endregion

        #region Ctor

        public GSCommerceNewsViewComponent(IHomeModelFactory homeModelFactory)
        {
            this._homeModelFactory = homeModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            try
            {
                //prepare model
                var model = _homeModelFactory.PrepareGSCommerceNewsModel();

                return View(model);
            }
            catch
            {
                return Content(string.Empty);
            }
        }

        #endregion
    }
}