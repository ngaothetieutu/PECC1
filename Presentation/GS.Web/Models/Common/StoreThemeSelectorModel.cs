using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class StoreThemeSelectorModel : BaseGSModel
    {
        public StoreThemeSelectorModel()
        {
            AvailableStoreThemes = new List<StoreThemeModel>();
        }

        public IList<StoreThemeModel> AvailableStoreThemes { get; set; }

        public StoreThemeModel CurrentStoreTheme { get; set; }
    }
}