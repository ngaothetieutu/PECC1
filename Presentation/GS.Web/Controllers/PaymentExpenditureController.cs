using GS.Core;
using GS.Core.Domain.Contracts;
using GS.Services.Catalog;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Services.Messages;
using GS.Services.PaymentAdvance;
using GS.Services.Security;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Factories;
using GS.Web.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class PaymentExpenditureController : BaseWorksController
    {
        #region Fiels
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractService _contractService;
        private readonly IContractModelFactory _contractModelFactory;
        private readonly IWorkContext _workContext;
        private readonly IContractFormService _contractFormService;
        private readonly IContractLogService _contractLogService;
        private readonly INotificationService _notificationService;
        private readonly ICustomerService _customerService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IConstructionService _constructionService;
        private readonly IUnitService _unitService;
        private readonly IPaymentAdvanceFactory _paymentAdvanceFactory;
        private readonly IPaymentAdvanceService _paymentAdvanceService;
        private readonly IContractPaymentService _contractPaymentService;
        private readonly ICurrencyService _currencyService;
        #endregion
        #region Ctor
        public PaymentExpenditureController(
            IContractMonitorService contractMonitorService,
            IConstructionModelFactory constructionModelFactory,
            IConstructionService constructionService,
            IPriceFormatter priceFormatter,
            IWorkTaskService workTaskService,
            ICustomerService customerService,
            IProcuringAgencyService procuringAgencyService,
            IContractLogService contractLogService,
            IContractTypeService contractTypeService,
            IContractFormService contractFormService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            IContractModelFactory contractModelFactory,
            IWorkContext workContext,
            IContractService contractService,
            INotificationService notificationService,
            IPrivateMessagesModelFactory privateMessagesModelFactory,
            IUnitService unitService,
            IPaymentAdvanceFactory paymentAdvanceFactory,
            IPaymentAdvanceService paymentAdvanceService,
             IContractPaymentService contractPaymentService,
             ICurrencyService currencyService)
        {
            this._constructionService = constructionService;
            this._priceFormatter = priceFormatter;
            this._workTaskService = workTaskService;
            this._customerService = customerService;
            this._notificationService = notificationService;
            this._contractLogService = contractLogService;
            this._contractFormService = contractFormService;
            this._contractModelFactory = contractModelFactory;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractService = contractService;
            this._workContext = workContext;
            this._unitService = unitService;
            this._paymentAdvanceFactory = paymentAdvanceFactory;
            this._paymentAdvanceService = paymentAdvanceService;
            this._contractPaymentService = contractPaymentService;
            this._currencyService = currencyService;
        }
        #endregion
        #region Methods
        public virtual IActionResult List()
        {
            var model = new PaymentExpenditureSearchModel();
            var treeTam = "";
            var unit = _unitService.GetAllUnits();
            model.AvailableUnit = unit.Select(c => new SelectListItem
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
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult List(PaymentExpenditureSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentExpenditureList))
                return AccessDeniedView();
            var model = _contractModelFactory.PreparePaymentExpenditureListModel(searchModel);
            return Json(model);
        }
        public virtual IActionResult Create(Guid ExpenditureGuid, int AdvanceId = 0, int AcceptanceId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentExpenditureCreate))
                return AccessDeniedView();
            var model = new PaymentExpenditureModel
            {
                ExpenditureGuid = ExpenditureGuid
            };
            if (AdvanceId > 0)
            {
                model.TypeId = (int)PaymentExpenditureType.Advance;
                var Advance = _paymentAdvanceService.GetPaymentAdvanceById(AdvanceId);
                model.UnitName = Advance.unit.Name;
                model.PaymentAdvanceId = AdvanceId;
                model.UnitId = (int)Advance.UnitId;
                model.TotalMoney = _priceFormatter.FormatPrice(Advance.TotalReceive.GetValueOrDefault(), true, Advance.currency);
                var acceptances = _contractService.getAllContractAcceptanceByAdvanceId(AdvanceId: AdvanceId);
                model.lstContractPayment = acceptances.Select(c => preparePaymentbyContractAcceptance(c)).ToList();
            }
            else if (AcceptanceId > 0)
            {
                var acceptances = _contractService.getContractAcceptancebyId(Id: AcceptanceId);
                model.UnitId = (int)acceptances.UnitId;
                model.UnitName = acceptances.unit.Name;
                model.AcceptanceId = AcceptanceId;
                model.TotalMoney = acceptances.TotalAmount.ToVNStringNumber();
                var AcceptanceSub = _contractService.GetallContractAcceptanceSubs(AcceptanceId:acceptances.Id,TypeId:(int)ContractAcceptancesType.NoiBo);
                model.lstContractPayment = AcceptanceSub.Select(c => preparePaymentbyContractAcceptanceSub(c)).ToList();
            }
            else
            {
                var paymentExpenditure = _contractService.GetPaymentExpenditureByGuid(model.ExpenditureGuid);
                model = paymentExpenditure.ToModel<PaymentExpenditureModel>();
                model.TotalMoney = model.TotalAmount.ToVNStringNumber();
                model.lstContractPayment = _contractPaymentService.GetAllContractPaymentsByExpenditureId(model.Id).Select(c => c.ToModel<ContractPaymentModel>()).ToList();
            };
             model.AvailableCurrency = _currencyService.GetAllCurrencies().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.CurrencyId
            }).ToList();
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Create(PaymentExpenditureModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentExpenditureCreate))
                return AccessDeniedView();
            if (ModelState.IsValid && model.lstContractPayment.Count() > 0)
            {
                var noti = "admin.common.Added";
                var item = new PaymentExpenditure();
                if (model.Id > 0)
                {
                    item = _contractService.GetPaymentExpenditureById(model.Id);
                }
                _contractModelFactory.PreparePaymentExpenditure(model, item);
                if (model.PaymentAdvanceId > 0)
                {
                    item.PaymentAdvanceId = model.PaymentAdvanceId;
                    item.AcceptanceId = null;
                }
                if (model.AcceptanceId > 0)
                {
                    item.AcceptanceId = model.AcceptanceId;
                    item.PaymentAdvanceId = null;
                }
                if (model.Id > 0)
                {
                    _contractPaymentService.DeleteContractPaymentsByExpenditureId(item.Id);
                    _contractService.UpdatePaymentExpenditure(item);
                    noti = "admin.common.Updated";
                }
                else
                {
                    _contractService.InsertPaymentExpenditure(item);
                }
                foreach (ContractPaymentModel paymentModel in model.lstContractPayment)
                {
                    var payment = new ContractPayment
                    {
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ApprovedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        IsReceipts = false,
                        StatusId =(int) ContractPaymentStatus.Approved,                       
                    };                  
                    var task = _workTaskService.GetTaskById(paymentModel.TaskId);
                    payment.ExpenditureId = item.Id;
                    payment.Description = item.Name + " " + task.Name;
                    payment.Code = item.Code + task.Code;
                    payment.TaskId = paymentModel.TaskId;
                    payment.ContractId = task.ContractId;
                    if (task.UnitId > 0)
                    {
                        payment.UnitId = (int)task.UnitId;
                    }
                    else
                    {
                        payment.UnitId = null;
                    }
                    if (task.CurrencyId > 0)
                    {
                        payment.CurrencyId = (int)task.CurrencyId;
                    }
                    else
                    {
                        payment.CurrencyId =(int) item.CurrencyId;
                    }
                    payment.Amount = paymentModel.Amount.ToNumber();                    
                    payment.TypeId = item.TypeId;
                    _contractPaymentService.InsertContractPayment(payment);
                }
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<PaymentExpenditureModel>());
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
        }
        public ContractPaymentModel preparePaymentbyContractAcceptance(ContractAcceptance Accep)
        {
            var model = new ContractPaymentModel();
            model.Amount = (Accep.TotalAmount.GetValueOrDefault(0) * Accep.Ratio.GetValueOrDefault(0)).ToVNStringNumber();
            var map = _contractService.GetAllContractAcceptanceTaskMapping(AcepptanceId: Accep.Id).FirstOrDefault();
            model.TaskId = map.task.Id;
            model.TaskName = map.task.Name;
            model.ContractName = map.task.contract.Name;
            return model;
        }
        public ContractPaymentModel preparePaymentbyContractAcceptanceSub(ContractAcceptanceSub acceptanceSub)
        {
            var model = new ContractPaymentModel();
            model.Amount = acceptanceSub.TotalAmount.GetValueOrDefault(0).ToVNStringNumber();
            model.TaskId = acceptanceSub.TaskId.GetValueOrDefault();
            model.TaskName = acceptanceSub.task.Name;
            model.ContractName = acceptanceSub.task.contract.Name;
            return model;
        }
        public virtual IActionResult _ContractPaymentAdd(int Id = 0)
        {
            var item = new PaymentExpenditure();
            var model = new PaymentExpenditureModel()
            {
                Id = Id
            };
            if (Id > 0)
            {
                item = _contractService.GetPaymentExpenditureById(Id);
                model = item.ToModel<PaymentExpenditureModel>();
            }

            return PartialView(model);
        }
        public virtual IActionResult DeletePaymentExpenditure(int Id)
        {
            var contractPayment = _contractPaymentService.GetContractPayments().Where(c => c.ExpenditureId == Id);
            if (contractPayment != null)
            {
                foreach (ContractPayment ct in contractPayment)
                {
                    _contractPaymentService.DeleteContractPayment(ct);
                }
            }
            var item = _contractService.GetPaymentExpenditureById(Id);
            if(item != null)
            {
                _contractService.DeletePaymentExpenditure(item);
            }
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), item.ToModel<PaymentExpenditureModel>());
        }
        #endregion
    }
}
