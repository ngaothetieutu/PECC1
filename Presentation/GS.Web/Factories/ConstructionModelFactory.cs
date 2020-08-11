using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Web.Infrastructure.Cache;
using GS.Web.Models.Directory;
using GS.Web.Models.Contracts;
using GS.Services.Contracts;
using GS.Web.Framework.Extensions;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Core.Domain.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GS.Web.Factories
{
    public class ConstructionModelFactory : IConstructionModelFactory
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        private readonly IConstructionTypeService _constructionTypeService;
        private readonly IConstructionCapitalService _constructionCapitalService;
        private readonly IConstructionService _constructionService;

        #endregion
        #region Ctor

        public ConstructionModelFactory(IConstructionTypeService constructionTypeService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IWorkContext workContext,
            IConstructionCapitalService constructionCapitalService,
            IConstructionService constructionService)
        {
            this._constructionTypeService = constructionTypeService;
            this._constructionCapitalService = constructionCapitalService;
            this._constructionService = constructionService;
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
        }

        #endregion
        public ConstructionTypeSearchModel PrepareConstructionTypeSearchModel(ConstructionTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ConstructionTypeListModel PrepareConstructionTypeListModel(ConstructionTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _constructionTypeService.GetConstructionTypeByName(searchModel.Name);

            //prepare list model
            var model = new ConstructionTypeListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ConstructionTypeModel>()),
                Total = items.Count
            };
            return model;
        }
        public ConstructionTypeModel PrepareConstructionTypeModel(ConstructionTypeModel model, ConstructionType item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ConstructionTypeModel>();
            }
            return model;
        }
        #region ConstructionCapital
        public ConstructionCapitalSearchModel PrepareConstructionCapitalSearchModel(ConstructionCapitalSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ConstructionCapitalListModel PrepareConstructionCapitalListModel(ConstructionCapitalSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _constructionCapitalService.GetConstructionCapitalsByName(searchModel.Name);

            //prepare list model
            var model = new ConstructionCapitalListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ConstructionCapitalModel>()),
                Total = items.Count
            };

            return model;
        }
        public ConstructionCapitalModel PrepareConstructionCapitalModel(ConstructionCapitalModel model, ConstructionCapital item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ConstructionCapitalModel>();
            }
            return model;
        }
        #endregion
        #region Construction
        public ConstructionSearchModel PrepareConstructionSearchModel(ConstructionSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();
            searchModel.lsType = _constructionTypeService.GetAllConstructionTypes().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == searchModel.TypeId
            }).ToList();
            searchModel.lsType.Insert(0, new SelectListItem()
            {
                Text = "Chọn loại công trình",
                Value = "0"
            });
            return searchModel;
        }

        public ConstructionListModel PrepareConstructionListModel(ConstructionSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _constructionService.GetContructionByName(searchModel.TypeId, searchModel.Name, searchModel.Page - 1, searchModel.PageSize);
            
            //prepare list model
            var model = new ConstructionListModel
            {
                //fill in model values from the entity
                Data = items.Select(store => store.ToModel<ConstructionModel>()),
                Total = items.TotalCount
            };
            return model;
        }
        public ConstructionModel PrepareConstructionModel(ConstructionModel model, Construction item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ConstructionModel>();
            }
            //tao cac dropdownlist
            model.AvailableTypes = _constructionTypeService.GetAllConstructionTypes().Select(c => new SelectListItem
            {
                Text=c.Name,
                Value=c.Id.ToString(),
                Selected=c.Id==model.TypeId
            }).ToList();
            model.AvailableCapitals = _constructionCapitalService.GetAllConstructionCapital().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.CapitalId
            }).ToList();
            return model;
        }
       public void PrepareConstruction(ConstructionModel model, Construction item)
        {
            //item.Id = model.Id;
            item.Code = model.Code;
            item.Name = model.Name;
            item.Description = model.Description;
            item.TypeId = model.TypeId;
            item.CapitalId = model.CapitalId;
        }
        #endregion
    }
}
