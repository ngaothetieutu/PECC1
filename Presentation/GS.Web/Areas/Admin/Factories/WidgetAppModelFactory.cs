using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GS.Core.Domain.WidgetApps;
using GS.Services.Localization;
using GS.Services.WidgetApps;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.WidgetApps;
using GS.Web.Framework.Extensions;
using GS.Web.Framework.Factories;

namespace GS.Web.Areas.Admin.Factories
{
    public class WidgetAppModelFactory : IWidgetAppModelFactory
    {
        #region Fields
        private readonly IWidgetAppService _widgetAppService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        #endregion
        #region Ctor
        public WidgetAppModelFactory(IWidgetAppService widgetAppService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory)
        {
            this._widgetAppService = widgetAppService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
        }
        #endregion
        #region
        public WidgetAppListModel PrepareWidgetAppListModel(WidgetAppSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var widgetAppModel = _widgetAppService.GetAllWidgetApps();

            var model = new WidgetAppListModel
            {
                Data = widgetAppModel.PaginationByRequestModel(searchModel).Select(attribute =>
                {
                    var widgetModel = attribute.ToModel<WidgetAppModel>();

                    return widgetModel;
                }),
                Total = widgetAppModel.Count
            };
            return model;
        }

        public virtual WidgetAppModel PrepareWidgetAppModel(WidgetAppModel model, WidgetApp widgetApp)
        {
            if (widgetApp != null)
            {
                model = model ?? widgetApp.ToModel<WidgetAppModel>();
            }

            return model;
        }
        #endregion
    }
}
