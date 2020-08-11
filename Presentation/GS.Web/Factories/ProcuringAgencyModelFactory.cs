using GS.Core;
using GS.Core.Caching;
using GS.Core.Domain.Contracts;
using GS.Services;
using GS.Services.Contracts;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Framework.Extensions;
using GS.Web.Models.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Factories
{
    public class ProcuringAgencyModelFactory : IProcuringAgencyModelFactory
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        private readonly IProcuringAgencyService _procuringAgencyService;


        #endregion
        #region Ctor

        public ProcuringAgencyModelFactory(IProcuringAgencyService procuringAgencyService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IWorkContext workContext)
        {
            this._procuringAgencyService = procuringAgencyService;

            this._countryService = countryService;
            this._localizationService = localizationService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
        }

        #endregion
        public ProcuringAgencySearchModel PrepareProcuringAgencySearchModel(ProcuringAgencySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();            
            searchModel.lsType= ((ProcuringAgencyType)searchModel.TypeId).ToSelectList();
            searchModel.IsInEVN = true;
            return searchModel;
        }

        public ProcuringAgencyListModel PrepareProcuringAgencyListModel(ProcuringAgencySearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _procuringAgencyService.GetProcuringAgencys(searchModel.TypeId,searchModel.Name,searchModel.IsInEVN,searchModel.Page-1,searchModel.PageSize);

            //prepare list model
            var model = new ProcuringAgencyListModel
            {
                //fill in model values from the entity
                Data = items.Select(c =>
                {
                    var m = c.ToModel<ProcuringAgencyModel>();
                    m.TypeText = _localizationService.GetLocalizedEnum(c.procuringAgencyType);
                    return m;
                }
                    ),
                Total = items.TotalCount
            };

            return model;
        }
        public ProcuringAgencyModel PrepareProcuringAgencyModel(ProcuringAgencyModel model, ProcuringAgency item, bool excludeProperties = false)
        {
            
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ProcuringAgencyModel>();
                model.TypeText = _localizationService.GetLocalizedEnum(item.procuringAgencyType);
            }
            //neu thong tin chu dau tu dang sua la PECC1 thi chi cho phep sua thong tin khac, thong tin loai chu dau tu khong cho sua
            if(model.procuringAgencyType==ProcuringAgencyType.PECC1)
                model.lsType = ((ProcuringAgencyType)model.TypeId).ToSelectList(true, new int[] { 0, (int)ProcuringAgencyType.Main, (int)ProcuringAgencyType.JointVenture });
            else
                model.lsType = ((ProcuringAgencyType)model.TypeId).ToSelectList(true, new int[] { 0, (int)ProcuringAgencyType.PECC1 });
            
            return model;
        }
    }
}
