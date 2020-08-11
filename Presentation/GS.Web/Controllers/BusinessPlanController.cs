using GS.Core;
using GS.Core.Domain.Advance;
using GS.Core.Domain.Contracts;
using GS.Services.Catalog;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.Localization;
using GS.Services.Messages;
using GS.Services.PaymentAdvance;
using GS.Services.Security;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Factories;
using GS.Web.Models.Contracts;
using GS.Web.Models.PaymentAdvance;
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class BusinessPlanController : BaseWorksController
    {
        #region       
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
        #endregion
        #region Ctor
        public BusinessPlanController(
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
            IPaymentAdvanceService paymentAdvanceService)
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
        }
        #endregion
        #region method
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public virtual IActionResult List()
        {
            var model = new ContractPaymentAdvanceSearchModel();
            var treeTam = "";
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
            return View(model);
        }
        public virtual IActionResult Delete(int Id)
        {
            var item = _paymentAdvanceService.GetPaymentAdvanceById(Id);
            if (item == null)
            {
                ErrorNotification("Không thể xoá");
                return RedirectToAction("List");
            }
            _paymentAdvanceService.deletePaymentAdvance(item);
            // SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Contract.Deleted"));
            var acceptances = _contractService.getAllContractAcceptanceByAdvanceId(Id);
            foreach (ContractAcceptance acceptance in acceptances)
            {
                _contractService.DeleteContractAcceptance(acceptance);
                _contractService.DeleteContractAcceptanceTaskMappingbyAcceptanceId(acceptance.Id);
            }
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Advance.Advance.Deleted"));
        }
        [HttpPost]
        public virtual IActionResult List(ContractPaymentAdvanceSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
            //    return AccessDeniedKendoGridJson();
            //prepare model
            var model = _paymentAdvanceFactory.PrepareContractPaymentAdvanceListModel(searchModel);
            return Json(model);
        }
        public virtual IActionResult Create(Guid AdvanceGuid)
        {
            var model = new ContractPaymentAdvanceModel();
            var item = _paymentAdvanceService.GetContractAdvanceByGuid(AdvanceGuid);
            if (item != null)
            {
                model = item.ToModel<ContractPaymentAdvanceModel>();
                model.TotalReceive = item.TotalReceive.ToVNStringNumber();
            }
            _paymentAdvanceFactory.PrepareContractPaymentAdvanceModel(model, null);
            if (item != null)
            {
                var listAcceptan = _contractService.getAllContractAcceptanceByAdvanceId(AdvanceId: item.Id);
                var listContract = listAcceptan.Select(c => c.ContractId).Distinct();
                model.ListContractId = String.Join(",", listContract.ToList());
            }
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Create(ContractPaymentAdvanceModel model)
        {
            var noti = "admin.common.Added";
            var item = new ContractPaymentAdvance();
            //var ListAcceptance = model.ListContractAcceptance.Where(c => c.TotalAmount.ToNumber() > 0).ToList();        

            if (ModelState.IsValid && model.ListContractAcceptance.Count > 0)
            {
                if (model.Id > 0)
                {
                    var acceptances = _contractService.getAllContractAcceptanceByAdvanceId(model.Id);
                    foreach (ContractAcceptance acceptance in acceptances)
                    {
                        _contractService.DeleteContractAcceptance(acceptance);
                        _contractService.DeleteContractAcceptanceTaskMappingbyAcceptanceId(acceptance.Id);
                    }
                    item = _paymentAdvanceService.GetPaymentAdvanceById(model.Id);
                    _paymentAdvanceFactory.PrepareContractPaymentAdvance(model, item);
                    _paymentAdvanceService.UpdatePaymentAdvance(item);
                    noti = "admin.common.Updated";
                }
                else
                {
                    _paymentAdvanceFactory.PrepareContractPaymentAdvance(model, item);
                    _paymentAdvanceService.InsertPaymentAdvance(item);
                }
                foreach (ContractAcceptanceModel accep in model.ListContractAcceptance)
                {
                    //add contractAcceptance
                    var accepitem = new ContractAcceptance();
                    accep.ApprovalDate = model.PayDate;
                    accep.PaymentAdvanceId = item.Id;
                    accep.TypeId = (int)ContractAcceptancesType.TamUng;
                    _contractModelFactory.PrepareContractAcceptance(accep, accepitem);
                    _contractService.InsertContractAcceptance(accepitem);
                    //add ContractAcceptamce_Task_Mapping
                    var AcceptanceTask = new ContractAcceptanceTaskMapping
                    {
                        TaskId = (int)accep.TaskId,
                       ContractAcceptanceId = accepitem.Id
                    };

                _contractService.InsertContractAcceptanceTaskMapping(AcceptanceTask);
            }
            return JsonSuccessMessage(_localizationService.GetResource(noti));
        }
        var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
    }
    public virtual IActionResult _AdvanceContracts(int UnitId)
    {
        var model = new ContractPaymentAdvanceModel();
        var Contracts = _contractService.getAllContractbyUnitId(UnitId);
        model.listContractModel = Contracts.Select(c => c.ToModel<ContractModel>()).ToList();
        return PartialView(model);
    }
    public virtual IActionResult _AdvanceContractTask(int ContractId, int UnitId, int AdvanceId = 0)
    {
        var Contract = _contractService.GetContractById(ContractId);
        if (Contract != null)
        {
            var model = _paymentAdvanceFactory.PrepareContractAdvanceModel(UnitId, Contract, AdvanceId);
            return PartialView(model);
        }
        else
        {
            return JsonErrorMessage("Eror");
        }
    }
    public virtual IActionResult _ContractAcceptanceAdd(int UnitId)
    {
        var model = new ContractAcceptanceModel()
        {
            UnitId = UnitId,
            TypeId = (int)ContractAcceptancesType.TamUng,
        };
        _contractModelFactory.PrepareContractAcceptanceModel(model, null);
        var Contracts = _contractService.getAllContractbyUnitId(UnitId);
        if (Contracts != null)
        {
            model.AvailableContract = Contracts.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            model.AvailableContract.Insert(0, new SelectListItem
            {
                Text = "---Chọn hợp đồng---",
                Value = "0"
            });
        }
        return PartialView(model);
    }
    public virtual IActionResult TaskContract(int ContractId = 0, int UnitId = 0)
    {
        var tasks = _workTaskService.getAllTaskbyUnit(ContractId: ContractId, UnitId: UnitId);
        var listAccep = tasks.Select(c => _paymentAdvanceFactory.prepareContractAcceptancebyTask(c)).ToList();
        return JsonSuccessMessage("ok", listAccep);
    }
    #endregion
}
}
