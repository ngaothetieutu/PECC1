using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Catalog;
using GS.Services.Configuration;
using GS.Services.Stores;
using GS.Web.Framework.Components;

namespace GS.Web.Areas.Admin.Components
{
    public class AclDisabledWarningViewComponent : GSViewComponent
    {
        private readonly CatalogSettings _catalogSettings;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;

        public AclDisabledWarningViewComponent(CatalogSettings catalogSettings,
            ISettingService settingService,
            IStoreService storeService)
        {
            this._catalogSettings = catalogSettings;
            this._settingService = settingService;
            this._storeService = storeService;
        }

        public IViewComponentResult Invoke()
        {
            //action displaying notification (warning) to a store owner that "ACL rules" feature is ignored

            //default setting
            var enabled = _catalogSettings.IgnoreAcl;
            if (!enabled)
            {
                //overridden settings
                var stores = _storeService.GetAllStores();
                foreach (var store in stores)
                {
                    var catalogSettings = _settingService.LoadSetting<CatalogSettings>(store.Id);
                    enabled = catalogSettings.IgnoreAcl;

                    if (enabled)
                        break;
                }
            }

            //This setting is disabled. No warnings.
            if (!enabled)
                return Content(string.Empty);

            return View();
        }
    }
}
