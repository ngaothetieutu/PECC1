using GS.Core.Domain.WidgetApps;
using GS.Web.Areas.Admin.Models.WidgetApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Areas.Admin.Factories
{
    public partial interface IWidgetAppModelFactory
    {
        WidgetAppModel PrepareWidgetAppModel(WidgetAppModel model, WidgetApp widgetApp);

        WidgetAppListModel PrepareWidgetAppListModel(WidgetAppSearchModel searchModel);
    }
}
