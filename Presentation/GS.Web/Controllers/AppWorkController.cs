using GS.Core;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Messages;
using GS.Core.Infrastructure;
using GS.Services.Catalog;
using GS.Services.Configuration;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Messages;
using GS.Services.Security;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Components;
using GS.Web.Factories;
using GS.Web.Models.Contracts;
using GS.Web.Models.PrivateMessages;
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class AppWorkController:BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractService _contractService;
        private readonly IContractModelFactory _contractModelFactory;
        private readonly IConstructionModelFactory _constructionModelFactory;
        private readonly IWorkContext _workContext;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IContractFormService _contractFormService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IContractLogService _contractLogService;
        private readonly INotificationService _notificationService;
        private readonly IPrivateMessagesModelFactory _privateMessagesModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IConstructionService _constructionService;
        private readonly IUnitService _unitService;
        private readonly IContractMonitorService _contractMonitorService;
        #endregion

        #region Ctor

        public AppWorkController(
            IContractMonitorService contractMonitorService,
            IUnitService unitService,
            IConstructionModelFactory constructionModelFactory,
            IConstructionService constructionService,
            IPriceFormatter priceFormatter,
            IWorkTaskService workTaskService,
            ICustomerService customerService,
            IProcuringAgencyService procuringAgencyService,
            IContractLogService contractLogService,
            IContractTypeService contractTypeService,
            IContractFormService contractFormService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IContractModelFactory contractModelFactory,
            IWorkContext workContext,
            IContractService contractService,
            INotificationService notificationService,
            IPrivateMessagesModelFactory privateMessagesModelFactory
            )
        {
            this._contractMonitorService = contractMonitorService;
            this._constructionModelFactory = constructionModelFactory;
            this._unitService = unitService;
            this._constructionService = constructionService;
            this._priceFormatter = priceFormatter;
            this._workTaskService = workTaskService;
            this._customerService = customerService;
            this._notificationService = notificationService;
            this._privateMessagesModelFactory = privateMessagesModelFactory;
            this._contractTypeService = contractTypeService;
            this._contractLogService = contractLogService;
            this._contractFormService = contractFormService;
            this._procuringAgencyService = procuringAgencyService;
            this._contractModelFactory = contractModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractService = contractService;
            this._workContext = workContext;
        }

        #endregion#

        #region fields
      

        #endregion
        
        public virtual IActionResult Index()
        {
            return View();
        }
        public virtual IActionResult _SumaryTotal()
        {
            ///lay thong tin tong gia tri hop dong
            var _totalMoney = _contractService.GetTotalMoney(0);
            ///lay thong tin tong so luong cong viec
            var _totalTask = _workTaskService.CountTask(_workContext.CurrentCustomer.Id);
            // lay thong tin NTKL
            var _totalMoneyAcceptance = _contractService.GetTotalMoneyAcceptance(0);
            // dov vi
            var _totalUnit = _unitService.GetTotalUnit();
            /// tong gia tri do dang chua NT
            var _totalUnfinish1 = _contractService.GetTotalMoneyContractunfinishByYear(DateTime.Now.Year);
            /// tong gia tri do dang chua thuc hien
            var _totalUnfinish2 = _contractService.GetUnfinish(DateTime.Now.Year);

            var model = new SumaryTotalModel
            {
                TotalMoney = _totalMoney.ToVNStringNumber(),
                TotalUnit = _totalUnit.ToVNStringNumber(),
                TotalMoneyAcceptance = _totalMoneyAcceptance.ToVNStringNumber(),
                TotalRemainingAmount = ((_totalMoney - _totalMoneyAcceptance) >= 0 ? (_totalMoney - _totalMoneyAcceptance) : 0).ToVNStringNumber(),
                TotalUnfinish1 = _totalUnfinish1.ToVNStringNumber(),
                TotalUnfinish2 = _totalUnfinish2.ToVNStringNumber()
            };
            return PartialView(model);
        }
        public virtual IActionResult _SumaryTotal2()
        {
            /// tong so tien da thanh toan
            var _totalPaymentPaid = _contractService.GetSumPaymentPaid();
            // tam ung
            var _totalPaymentAdvance = _contractService.GetSumPaymentAdvance(0);
            // cong no phai thu
            var _totalReceivable = _contractService.GetTotalMoneyAcceptance(0) - _contractService.GetSumPaymentPaid();
            // so tien chua thu hoi
            var _totalRequest = _contractService.GetTotalRequest() - _contractService.GetSumPaymentPaid();

            var model = new SumaryTotalModel2
            {
                TotalPaymentPaid = _totalPaymentPaid.ToVNStringNumber(),
                TotalPaymentAdvance = _totalPaymentAdvance.ToVNStringNumber(),
                TotalReceivable = _totalReceivable >= 0 ? _totalReceivable.ToVNStringNumber() : 0.ToVNStringNumber(),
                TotalRequest = _totalRequest >= 0 ? _totalRequest.ToVNStringNumber() : 0.ToVNStringNumber()
            };
            return PartialView(model);
        }
        public virtual IActionResult _SumaryTotal3()
        {
            var _totalProcuringAgency = _procuringAgencyService.GetTotalProcuringAgency(0);
            var _totalProcuringAgencyIsEVN = _procuringAgencyService.GetTotalProcuringAgencyIsInEVN(true);
            ///lay thong tin tong so luong hop dong
            var _totalContract = _contractService.GetContracts(CustomerId: _workContext.CurrentCustomer.Id, getOnlyTotalCount: true, StartYear: 0, classificationId: ContractClassification.AB);
            ///lay thong tin tong so luong cong trinh
            var _totalConstruction = _constructionService.GetTotalConstruction(0);
            // nhan vien
            var _totalCustomer = _customerService.GetTotalCustomer();

            var model = new SumaryTotalModel3
            {
                TotalProcuringAgency = _totalProcuringAgency.ToVNStringNumber(),
                TotalContract = _totalContract.TotalCount.ToVNStringNumber(),
                TotalConstruction = _totalConstruction.ToVNStringNumber(),
                TotalProcuringAgencyIsEVN = _totalProcuringAgencyIsEVN.ToVNStringNumber(),
                totalCustomer = _totalCustomer.ToVNStringNumber()
            };
            return PartialView(model);
        }
        public virtual IActionResult _SumaryTotalByYear(int time)
        {
            ///lay thong tin tong gia tri hop dong
            var _totalMoneyByYear = _contractService.GetTotalMoney(time);
            // lay thong tin NTKL
            var _totalMoneyAcceptanceByYear = _contractService.GetTotalMoneyAcceptance(time);
            // tam ung
            var _totalPaymentAdvanceByYear = _contractService.GetSumPaymentAdvance(time);
            ///lay thong tin tong so luong cong trinh
            //var _totalConstructionByYear = _constructionService.GetTotalConstruction(time);
            /// thong tin chu dau tu
            //var _totalProcuringAgencyByYear = _procuringAgencyService.GetTotalProcuringAgency(time);
            ///lay thong tin tong so luong hop dong
            var _totalContractByYear = _contractService.GetContracts(CustomerId: _workContext.CurrentCustomer.Id, getOnlyTotalCount: true, StartYear: time, classificationId: ContractClassification.AB);
            /// tong so tien da thanh toan
            var _totalPaymentPaidByYear = _contractService.GetSumPaymentPaid(time);
            /// gia tri do dang theo nam
            var _totalMoneyContractunfinishByYear = _contractService.GetTotalMoneyContractunfinishByYear(time);
            /// gia tri do dang chua thuc hien
            var _totalMoneyContractUnfinish2ByYear = _contractService.GetUnfinish(time);
            var model = new SumaryTotalByYearModel
            {
                Time = time,
                TotalMoney = _totalMoneyByYear.ToVNStringNumber(),
                TotalMoneyAcceptance = _totalMoneyAcceptanceByYear.ToVNStringNumber(),
                TotalPaymentAdvance = _totalPaymentAdvanceByYear.ToVNStringNumber(),
                TotalRemainingAmount = (_totalMoneyByYear - _totalMoneyAcceptanceByYear).ToVNStringNumber(),
                TotalProcuringAgency = "0",
                //_totalProcuringAgencyByYear.ToVNStringNumber(),
                TotalContract = _totalContractByYear.TotalCount.ToVNStringNumber(),
                TotalConstruction = "0",
                //_totalConstructionByYear.ToVNStringNumber(),
                TotalPaymentPaid = _totalPaymentPaidByYear.ToVNStringNumber(),
                TotalMoneyContractunfinish = _totalMoneyContractunfinishByYear.ToVNStringNumber(),
                TotalMoneyContractUnfinish2 = _totalMoneyContractUnfinish2ByYear.ToVNStringNumber()
            };
            model.Time = time;
            return PartialView(model);
        }
        public virtual IActionResult _ListContractUnfinish(int year)
        {
            var model = new ContractUnfinishListModel();
            var items = _contractService.GetAllContractunfinishByYear(year);
            model.Data = items.Select(c => c.ToModel<ContractUnfinishModel>()).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _TopFiveConstruction()
        {
            var items = _contractService.GetTopConstructionContract().Select(c => {
                var m = c.ToModel<ConstructionModel>();
                return m;
            }).ToList();

            var model = new TopFiveConstruction
            {
                topFiveConstruction = items
            };
            return PartialView(model);
        }
        public virtual IActionResult _TopContractDealine()
        {
            var items = _contractService.GetTopContractDealine().Select(c=> {
                var m = c.ToModel<ContractModel>();
                _contractModelFactory.PrepareContractUnfinish(m);
                var contractMonitor = _contractService.GetContractMonitor(c.Id);
                if (contractMonitor != null)
                {
                    m.contractMonitor = contractMonitor.ToModel<ContractMonitorModel>();
                }
                return m;
            }).ToList();

            var model = new TopContractDealine
            {
                topContractDealine = items
            };

            return PartialView(model);
        }
        public virtual IActionResult _DelayedProcessing()
        {
            var items = _contractService.GetContractDelayedProcessing().Select(c => {
                var m = c.ToModel<ContractModel>();
                _contractModelFactory.PrepareContractUnfinish(m);
                var contractMonitor = _contractService.GetContractMonitor(c.Id);
                if (contractMonitor != null)
                {
                    m.contractMonitor = contractMonitor.ToModel<ContractMonitorModel>();
                }
                return m;
            }).ToList();
            var model = new ContractDelayedProcesing
            {
                contractDelayedProcessing = items
            };

            return PartialView(model);
        }
        public JsonResult GetNotification()
        {
            //var username = _workContext.CurrentCustomer.Username;
            //var id = _workContext.CurrentCustomer.Id;
            var list = _notificationService.GetNotifications(_workContext.CurrentCustomer.Id)
                .Where(c=>c.notificationStatus==NotificationStatus.New);
            return Json(list);
        }
        public virtual IActionResult GetMenuBadge()
        {
            //lay thong tin hop dong theo user hien tai
            //tam thoi lay tat ca
            var _totalContract = _contractService.GetContracts(CustomerId:_workContext.CurrentCustomer.Id,getOnlyTotalCount:true,classificationId: ContractClassification.AB);
            ///lay so luong cong viec
            ///
            var _totalTask = _workTaskService.CountTask(_workContext.CurrentCustomer.Id);

            //lay so luong nhan su
            var roleids = new int[] { GSCustomerDefaults.RegisteredRoleId };
            var _totalCustomer = _customerService.GetAllCustomers(getOnlyTotalCount: true,customerRoleIds: roleids).TotalCount;
            ///lay thong tin so luong cho menu
            ///
            var model = new
            {
                BadgeContract = _totalContract.TotalCount,
                BadgeTask = _totalTask,
                BadgeCustomer = _totalCustomer
            };
            return Json(model);
        }
        public virtual IActionResult _Notification(int skip)
        {
            var items = _notificationService.GetMoreNotification(_workContext.CurrentCustomer.Id, 10, skip);
            var model = items.Select(store => store.ToModel<NotificationModel>()).ToList();
            return PartialView(model);
        }
        public virtual IActionResult Notification()
        {
            var searchModel = new NotificationSearchModel();
            searchModel.CustomerId = _workContext.CurrentCustomer.Id;
            var model = _privateMessagesModelFactory.PrepareNotificationListModel(searchModel);
            return View(model);
        }
        public virtual IActionResult _DelayedPayment()
        {
            var items = _contractMonitorService.GetContractsMonitorDelayPayment().Select(c => {
                var m = c.ToModel<ContractModel>();
                _contractModelFactory.PrepareContractUnfinish(m);
                var contractMonitor = _contractService.GetContractMonitor(c.Id);
                if (contractMonitor != null)
                {
                    m.contractMonitor = contractMonitor.ToModel<ContractMonitorModel>();
                }
                return m;
            }).ToList();

            var model = new DelayPayment
            {
                contractDelayPayment = items
            };

            return PartialView(model);
        }
        public virtual IActionResult _TotalTypeContract()
        {
            var model = _contractService.GetTotalTypeContract(0).ToList();          
            //TaskId.toEntity<ContractPaymentPlanRule>()
            return PartialView(model);
        }
        public virtual IActionResult _TotalTypeContractByYear(int time)
        {
            var model = _contractService.GetTotalTypeContractByYear(time, 0).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _BarCharts()
        {
            return PartialView();
        }
        public virtual JsonResult _ContractStaticData()
        {
            var listModel = new List<BarCharts>();
            for (int i = DateTime.Now.Year - 8; i <= DateTime.Now.Year; i++)
            {
                ///lay thong tin tong gia tri hop dong
                var _totalMoneyByYear = _contractService.GetTotalMoney(i);
                ///lay thong tin NTKL
                var _totalMoneyAcceptanceByYear = _contractService.GetTotalMoneyAcceptance(i);

                BarCharts model = new BarCharts();
                model.AxisLabel = i.ToString();
                model.Val1 = _totalMoneyByYear;
                model.Val2 = _totalMoneyAcceptanceByYear;
                listModel.Add(model);
            }

            return Json(listModel);
        }
        public virtual IActionResult _ListTotalContractInYear (int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYear(year);
            model.Data = items.Select(c => c.ToModel<ContractModel>()).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractAcceptanceInYear(int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearAcceptance(year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractInYearPaymentAdvance(int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearPaymentAdvance(year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractPaymentPaidInYear(int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearPaymentPaid(year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractByConstructionType(int constructionTypeId, int year)
        {
            
            var model = new ContractListModel();
            var items = _contractService.GetAllContractByConstructionInYear(constructionTypeId, year);
            model.Data = items.Select(c => c.ToModel<ContractModel>());
            return PartialView(model);
        }
        public virtual IActionResult _ListContractAcceptanceInYearByConstructionTypeId(int constructionTypeId, int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearPaymentByConstructionTypeId(constructionTypeId, year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractPaymentAdvanceInYearByConstructionTypeId(int constructionTypeId, int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearPaymentAdvanceByConstructionTypeId(constructionTypeId, year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ListContractPaymentPaidInYearByConstructionTypeId(int constructionTypeId, int year)
        {
            var model = new ContractListModel();
            var items = _contractService.GetAllContractInYearPaymentPaidByConstructionTypeId(constructionTypeId, year);
            model.Data = items.Select(c => _contractModelFactory.PrepareDashBoardThongKe(c, null, year)).ToList();
            return PartialView(model);
        }
    }
}
