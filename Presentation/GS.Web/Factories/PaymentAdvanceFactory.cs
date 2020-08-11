using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Web.Infrastructure.Cache;
using GS.Web.Models.Directory;
using GS.Web.Models.Works;
using GS.Services.Works;
using GS.Web.Framework.Extensions;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Core.Domain.Works;
using GS.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Services.Customers;
using GS.Services.Contracts;
using GS.Web.Models.Contracts;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Services.Media;
using GS.Core.Domain.Media;
using GS.Services.Common;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Services.Catalog;
using GS.Core.Domain.Advance;
using GS.Web.Models.PaymentAdvance;
using GS.Services.PaymentAdvance;

namespace GS.Web.Factories
{
    public class PaymentAdvanceFactory : IPaymentAdvanceFactory
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IWorkTaskService _workTaskService;
        private readonly ITaskGroupService _taskGroupService;
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
        private readonly IContractLogService _contractLogService;
        private readonly IUnitService _unitService;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IContractPaymentService _contractPaymentService;
        private readonly IPaymentAdvanceService _paymentAdvanceService;
        #endregion
        #region Ctor

        public PaymentAdvanceFactory(ICurrencyService currencyService,
            ITaskGroupService taskGroupService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IWorkContext workContext,
            IWorkTaskService workTaskService,
            ICustomerService customerService,
            IContractService contractService,
            IContractLogService contractLogService,
            IUnitService unitService,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            IPriceFormatter priceFormatter,
        IGenericAttributeService genericAttributeService,
        IContractPaymentService contractPaymentService,
        IPaymentAdvanceService paymentAdvanceService
        )
        {
            this._currencyService = currencyService;
            this._taskGroupService = taskGroupService;
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
            this._workTaskService = workTaskService;
            this._customerService = customerService;
            this._contractService = contractService;
            this._contractLogService = contractLogService;
            this._unitService = unitService;
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
            this._genericAttributeService = genericAttributeService;
            this._priceFormatter = priceFormatter;
            this._contractPaymentService = contractPaymentService;
            this._paymentAdvanceService = paymentAdvanceService;
        }


        #endregion
        #region mothod     

        public TaskGroupSearchModel PrepareTaskGroupSearchModel(ContractPaymentAdvanceSearchModel searchModel)
        {
            throw new NotImplementedException();
        }
        public void PrepareContractPaymentAdvance(ContractPaymentAdvanceModel model, ContractPaymentAdvance item)
        {
            item.StatusId = (int)PaymentAdvanceStatus.Use;
            if (model.Id ==0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
                item.AdvanceGuid = Guid.NewGuid();
            }           
            item.Name = model.Name;
            item.Code = model.Code;
            item.TotalAmount = model.TotalAmount.ToNumber();
            item.TotalReceive = model.TotalReceive.ToNumber();
            item.Description = model.Description;
            if (model.UnitId>0)
            {
                item.UnitId = model.UnitId;
            }            
            item.PayDate = model.PayDate;
            if (model.CurrencyId >0)
            {
                item.CurrencyId = model.CurrencyId;
            }
        }
        public ContractPaymentAdvanceListModel PrepareContractPaymentAdvanceListModel(ContractPaymentAdvanceSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _paymentAdvanceService.getAllPaymentAdvance(Keysearch: searchModel.Keysearch, UnitId: (int)searchModel.UnitId, pageSize: searchModel.PageSize, pageIndex: searchModel.Page - 1, FromDate: searchModel.FromDate, ToDate: searchModel.ToDate);

            //prepare list model
            var model = new ContractPaymentAdvanceListModel
            {
                //fill in model values from the entity
                Data = items.Select(store => store.ToModel<ContractPaymentAdvanceModel>()),
                Total = items.TotalCount
            };
            return model;
        }

        public void PrepareContractPaymentAdvanceModel(ContractPaymentAdvanceModel model, ContractPaymentAdvance item)
        {
            string treeTam = "";
            var units = _unitService.GetAllUnits();
            model.AvailableUnit = units.Select(c => new SelectListItem
            {
                Text = treeTam.PadLeft((int)c.TreeLevel - 1, '-') + c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.UnitId
            }).ToList();
            model.AvailableUnit.Insert(0,
                new SelectListItem
                {
                    Value = "0",
                    Text = "--Chọn đơn vị--"
                });
            var currencys = _currencyService.GetAllCurrencies();
            model.listCurrency = currencys.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.CurrencyId
            }).ToList();
            //model.listCurrency.Insert(0, new SelectListItem
            //{
            //    Value = "0",
            //    Text = "--Chọn đơn vị tiền---"
            //});
        }
        public ContractPaymentAdvanceSearchModel PrepareContractPaymentAdvanceSearchModel(ContractPaymentAdvanceSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public ContractAcceptanceModel prepareContractAcceptancebyTask(Task task)
        {
            var model = new ContractAcceptanceModel
            {
                TaskName = task.Name                
            };
            model.TaskId = task.Id;
            model.ContractId = task.ContractId;
            model.TaskTotalMoney = task.TotalMoney;
            model.TaskTotalMoneyText = _priceFormatter.FormatPrice(task.TotalMoney.GetValueOrDefault(0), true, task.currency);
            //so da tam ung
            var acceps = _contractService.getAllContractAcceptance(ContractId: task.ContractId, TaskId: task.Id, TypeId: (int)ContractAcceptancesType.TamUng).ToList();
            model.ToltalAmountAdvanced = acceps.Sum(c => (Int64)c.TotalAmount);
            model.SumToltalAmount = (Int64)task.TotalMoney;
            model.ToltalAmountAdvancedText = _priceFormatter.FormatPrice(model.ToltalAmountAdvanced, true, task.currency);
            return model;
        }
        public ContractAcceptanceModel PrepareContractAcceptance(ContractAcceptance item)
        {
            var mapping = _contractService.GetAllContractAcceptanceTaskMapping(AcepptanceId: item.Id).FirstOrDefault();
            var task = _workTaskService.GetTaskById(mapping.TaskId);
            var model = new ContractAcceptanceModel
            {
                TaskName = task.Name
            };
            model.TaskId = task.Id;
            model.ContractId = task.ContractId;
            model.TaskTotalMoneyText = _priceFormatter.FormatPrice(task.TotalMoney.GetValueOrDefault(0), true, task.currency);
            //so da tam ung
            var acceps = _contractService.getAllContractAcceptance(ContractId: task.ContractId, TaskId: task.Id, TypeId: (int)ContractAcceptancesType.TamUng).Where(c=>c.Id != item.Id).ToList();
            model.ToltalAmountAdvanced = acceps.Sum(c => (Int64)c.TotalAmount);
            model.SumToltalAmount = (Int64)task.TotalMoney;
            model.TotalAmount = item.TotalAmount.ToVNStringNumber();
            model.Ratio = item.Ratio * 100;
            model.ToltalAmountAdvancedText = _priceFormatter.FormatPrice(model.ToltalAmountAdvanced, true, task.currency);
            return model;
        }
        public ContractModel PrepareContractAdvanceModel(int UnitId, Contract contract,int AdvanceId = 0)
        {
            var model = contract.ToModel<ContractModel>();
            if (AdvanceId>0)
            {
                var Acceptances = _contractService.getAllContractAcceptanceByAdvanceId(AdvanceId: AdvanceId,ContractId:contract.Id);
                model.lstContractAcceptance = Acceptances.Select(c => PrepareContractAcceptance(c)).ToList();
            }
            else
            {
                var task = _workTaskService.getAllTaskbyUnit(UnitId: UnitId, ContractId: contract.Id);
                model.lstContractAcceptance = task.Select(c => prepareContractAcceptancebyTask(c)).ToList();
            }           
            return model;
        }
        #endregion

    }
}
