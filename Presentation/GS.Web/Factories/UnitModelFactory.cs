using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Web.Models.Directory;
using GS.Web.Framework.Extensions;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Services.Stores;
using GS.Services.Customers;
using GS.Core.Domain.Customers;
using GS.Web.Models.Unit;

namespace GS.Web.Factories
{
    public class UnitModelFactory:IUnitModelFactory
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        
        private readonly IUnitService _unitService;

        #endregion
        #region Ctor

        public UnitModelFactory(ICustomerService customerService,
            IUnitService unitService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IWorkContext workContext)
        {
            this._customerService = customerService;
            this._unitService = unitService;
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
        }

        #endregion
        #region Unit
        public UnitSearchModel PrepareUnitSearchModel(UnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();
            searchModel.getUnitModel.ddlParentId = _unitService.GetAllUnits().Select(c => new SelectListItem
            {
                Text = c.LevelSpace + " " + c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == searchModel.getUnitModel.ParentId
            }).ToList();
            searchModel.getUnitModel.ddlParentId.Insert(0, new SelectListItem { Value = "0", Text = "----- Chọn đơn vị cha -----" });

            return searchModel;
        }

        public UnitListModel PrepareUnitListModel(UnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _unitService.GetAllUnits();

            //prepare list model
            var model = new UnitListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<UnitModel>()),
                Total = items.Count
            };

            return model;
        }
        public List<UnitModel> PrepareUnitTreeListModel(UnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get items
            var items = _unitService.GetAllUnits(searchModel.Name,searchModel.ParentId);
            var models = new List<UnitModel>();
            //lay tat ca model co parent id =null
            models = items.Where(c => !c.ParentId.HasValue).Select(u => u.ToModel<UnitModel>()).ToList();
            foreach (var m in models)
                SubPrepareUnitTreeListModel(m, items);
            return models;        
        }
        private void SubPrepareUnitTreeListModel(UnitModel parentModel, IList<Unit> allUnits)
        {
            var subUnits = allUnits.Where(c => c.ParentId == parentModel.Id).ToList();
            foreach(var sub in subUnits)
            {
                var submodel = sub.ToModel<UnitModel>();
                //goi de quy de tim con tiep
                SubPrepareUnitTreeListModel(submodel, allUnits);
                //add model con vao danh sach cua cha
                parentModel.subUnitModels.Add(submodel);
            }
        }
        public UnitModel PrepareUnitModel(UnitModel model, Unit item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<UnitModel>();
            }
            model.ddlParentId = _unitService.GetAllUnits().Select(c => new SelectListItem
            {                
                Text = c.LevelSpace + " " + c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.ParentId
            }).ToList();
            model.ddlParentId.Insert(0, new SelectListItem { Value = "0", Text = "----- Chọn đơn vị cha -----" });
            return model;
        }
        public CustomerUnitModel PrepareCustomerUnitModel(CustomerUnitModel model, CustomerUnitMapping item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<CustomerUnitModel>();
            }

            return model;
        }
        public CustomerUnitListModel PrepareCustomerUnitListModel(CustomerUnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _unitService.GetAllCustomerByUnitId(searchModel.UnitId);

            //prepare list model
            var model = new CustomerUnitListModel
            {
                //fill in model values from the entity
                Data = items.Select(c => {
                    var m = new CustomerUnitModel();
                    m.CustomerId = c.Id;
                    m.Username = c.Username;
                    m.FullName = _customerService.GetCustomerFullName(c);
                    return m;
                }).ToList(),
                Total = items.Count
            };

            return model;
        }
        public CustomerUnitSearchModel PrepareCustomerCheckListModel(CustomerUnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _unitService.GetAllCustomerByUnitId(searchModel.UnitId);
            CustomerUnitSearchModel model = new CustomerUnitSearchModel();
            //prepare list model
            model.ListCustomer = items.Select(c =>
            {
                var m = new CustomerUnitModel();
                m.CustomerId = c.Id;
                m.Username = c.Username;
                m.FullName = _customerService.GetCustomerFullName(c);
                m.Email = c.Email;
                return m;
            }).ToList();

            return model;
        }
        public CustomerUnitSearchModel PrepareCustomerSearchListModel(CustomerUnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var param = _unitService.GetAllCustomerByUnitId(searchModel.UnitId).Select(c=>c.Id).ToArray();            
            //get items
            var items = _customerService.GetSearchAllCustomer(Keyword: searchModel.srKeywordCustomer, ExcludeCustomerIds: param);

            //prepare list model
            CustomerUnitSearchModel model = new CustomerUnitSearchModel();
            model.ListCustomer = items.Select(c =>
            {
                var m = new CustomerUnitModel();
                m.CustomerId = c.Id;
                m.Username = c.Username;
                m.FullName = _customerService.GetCustomerFullName(c);
                m.Email = c.Email;
                return m;
            }).ToList();

            return model;
        }
        public CustomerUnitSearchModel PrepareCustomerUnitSearchModel(CustomerUnitSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }
        #endregion
    }
}
