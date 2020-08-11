using GS.Core;
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
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class ContractAcceptanceController : BaseWorksController
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
        public ContractAcceptanceController(IContractMonitorService contractMonitorService,
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
        public virtual IActionResult List()
        {
            //prepare model
            var model = _contractModelFactory.PrepareContractAcceptanceSearchModel(new ContractAcceptanceSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ContractAcceptanceSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcceptanceList))
                return AccessDeniedView();

            //prepare model
            var model = _contractModelFactory.PrepareContractAcceptanceNoiBoListModel(searchModel);
            return Json(model);
        }
        public virtual IActionResult Create(Guid acceptanceGuid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcceptanceCreate))
                return AccessDeniedView();
            var model = new ContractAcceptanceModel();
            var item = _contractService.GetContractAcceptanceByGuid(acceptanceGuid);
            if (item != null)
            {
                model = item.ToModel<ContractAcceptanceModel>();
                model.TotalAmount = item.TotalAmount.ToVNStringNumber();
            }
            _contractModelFactory.PrepareContracAcceptanceNoiBoModel(model, null);
            if (model.Id > 0)
            {
                var lstContractAcceptanceSub = _contractService.GetAllContractAcceptanceSubByAcceptanceNoiBoId(item.Id);
                var lstContractId = lstContractAcceptanceSub.Select(c => c.ContractId).Distinct();
                model.ListContractId = String.Join(",", lstContractId.ToList());
                //get list file
                var ListFileId = _contractService.GetContractFiles(TypeId: (int)ContractFileType.AcceptanceNoiBo, EntityId: item.Id);
                model.WorkFileIds = String.Join(",", ListFileId.Select(c => c.FileId).ToList());
            }
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Create(ContractAcceptanceModel model)
        {
            var noti = "admin.common.Added";
            var item = new ContractAcceptance();
            if (ModelState.IsValid && model.listAcceptanceSub.Count > 0)
            {
                if (model.Id > 0)
                {
                    item = _contractService.getContractAcceptancebyId(model.Id);
                    _contractModelFactory.PrepareContractAcceptanceNoiboForUnit(model, item);
                    //remove listAcceptanceSub cu
                    _contractService.DeleteContractAcceptanceSubByAcceptanceId(AccepId: model.Id, TypeId: (int)ContractAcceptancesType.NoiBo);
                }
                // add ContractAcceptanceNoibo
                _contractModelFactory.PrepareContractAcceptanceNoiboForUnit(model, item);
                if (model.Id > 0)
                {
                    _contractService.UpdateContractAcceptance(item);
                    // Insert ContractLog
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractAcceptanceModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Sửa nghiệm thu nội bộ: " + item.Name);
                    noti = "admin.common.updated";
                }
                else
                {
                    _contractService.InsertContractAcceptance(item);
                    // Insert ContractLog
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractAcceptanceModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Thêm mới nghiệm thu nội bộ: " + item.Name);
                }
                //var _cas = new ContractAcceptanceSub();
                foreach (ContractAcceptanceSubModel _casModel in model.listAcceptanceSub)
                {
                    _casModel.Description = item.Name;
                    var _cas = _contractModelFactory.PrepareContractAcceptanceSubByTask(_casModel, null);
                    _cas.AcceptanceId = item.Id;
                    _contractService.InsertContractAcceptionSub(_cas);
                    if (model.SelectedListFileId.Count > 0)
                    {
                        foreach (int FileId in model.SelectedListFileId)
                        {
                            var contractFile = new ContractFile
                            {
                                ContractId = _cas.ContractId,
                                FileId = FileId,
                                TypeId = (int)ContractFileType.AcceptanceNoiBo,
                                EntityId = item.Id.ToString(),
                            };
                            _contractService.InsertContractFile(contractFile);
                        }
                    }
                }
                return JsonSuccessMessage(_localizationService.GetResource(noti));
            }
            else
            {
                var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
                return JsonErrorMessage("Error", list);
            }
        }
        public virtual IActionResult _AcceptanceContracts(int UnitId, string ApprovalDateString)
        {
            var model = new ContractAcceptanceModel();
            if (ApprovalDateString == null)
            {
                ApprovalDateString = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var Contracts = _contractService.GetAllContractPaymentAcceptanceByUnitId(UnitId, ApprovalDateString);
            model.listContractModel = Contracts.Select(c => {
                var m = new ContractModel
                {
                    Name = c.Name,
                    Id = c.Id,
                    ConstructionName = _constructionService.GetConstructionById((int)c.ConstructionId).Name
                };
                return m;
            }).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _AcceptanceContractTask(string approvalDateString, int ContractId, int unitId, int acceptanceId = 0 )
        {
            if (approvalDateString == null)
            {
                approvalDateString = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var Contract = _contractService.GetContractById(ContractId);
            var model = Contract.ToModel<ContractModel>();
            if (acceptanceId > 0)
            {
                var lstAcceptanceSub = _contractService.GetAllContractAcceptanceNoiBoSub(ContractId, unitId, acceptanceId);
                model.lstContractAcceptanceSub = lstAcceptanceSub.Select(c => _contractModelFactory.PrepareContarctAcceptanceSubModelByAcceptanceSub(c)).ToList();
            }
            else
            {
                var lstTasks = _workTaskService.GetAllTasksByAprovalDatePaymentAcceptance(unitId, ContractId, approvalDateString);
                model.lstContractAcceptanceSub = lstTasks.Select(c => _contractModelFactory.PrepareContractAcceptanceSubByTaskModel(c.Id, approvalDateString)).ToList();
            }
            return PartialView(model);
        }
        public virtual IActionResult DeleteContractAcceptanceNoiBo(int Id)
        {
            var item = _contractService.GetContractAcceptanceNoiBoById(Id);
            if (item == null)
            {
                ErrorNotification("Không thể xoá");
                return RedirectToAction("List");
            }
            _contractService.DeleteContractAcceptance(item);
            // Insert ContractLog
            var contractLog = new ContractLog()
            {
                ContractId = item.Id,
                CreatedDate = DateTime.Now,
                CreatorId = _workContext.CurrentCustomer.Id,
                ContractData = item.ToModel<ContractAcceptanceModel>().toStringJson()
            };
            _contractLogService.InsertContractLog(contractLog, "Xóa nghiệm thu nội bộ: " + item.Name);
            var lstAcceptanceSub = _contractService.GetAllContractAcceptanceSubByAcceptanceNoiBoId(item.Id);
            foreach (ContractAcceptanceSub acceptanceSub in lstAcceptanceSub)
            {
                _contractService.DeleteContractAcceptanceSub(acceptanceSub);
            }
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contract.ContractAcceptance.Deleted"));
        }
    }
}
