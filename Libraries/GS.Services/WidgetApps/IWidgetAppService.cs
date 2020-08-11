using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.WidgetApps;

namespace GS.Services.WidgetApps
{
    public interface IWidgetAppService
    {
        void InsertWidgetApp(WidgetApp item);
        void UpdateWidgetApp(WidgetApp item);
        void DeleteWidgetApp(WidgetApp item);
        WidgetApp GetWidgetAppById(int widgetAppId);
        IList<WidgetApp> GetAllWidgetApps();
    }
}
