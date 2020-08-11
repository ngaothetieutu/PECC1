using GS.Core;
using GS.Core.Configuration;
using GS.Core.Data;
using GS.Core.Domain.Common;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using GS.Data;
using GS.Services;
using GS.Services.Catalog;
using GS.Services.Contracts;
using GS.Services.Directory;
using GS.Services.Localization;
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
using System.Data;
using System.Linq;
//using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public class WorksController : BaseWorksController
    {
        #region Fields
        private readonly IWorkTaskService _workTaskService;
        private readonly IContractLogService _contractLogService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly ITaskModelFactory _taskModelFactory;
        private readonly IWorkModelFactory _workModelFactory;
        private readonly IContractModelFactory _contractModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IContractService _contractService;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly GSConfig _config;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IPriceFormatter _priceFormatter;
        #endregion
        #region ctor
        public WorksController(IWorkTaskService workTaskService,
            IContractLogService contractLogService,
            CurrencySettings currencySettings,
            ICurrencyService currencyService,
            IWorkModelFactory workModelFactory,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext,
            ITaskModelFactory taskModelFactory,
            IContractModelFactory contractModelFactory,
             IContractService contractService,
             GSConfig config,
             IDataProvider dataProvider,
             IProcuringAgencyService procuringAgencyService,
             IDbContext dbContext,
             IPriceFormatter priceFormatter
            )
        {
            this._workModelFactory = workModelFactory;
            this._contractLogService = contractLogService;
            this._currencySettings = currencySettings;
            this._localizationService = localizationService;
            this._workTaskService = workTaskService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._taskModelFactory = taskModelFactory;
            this._contractModelFactory = contractModelFactory;
            this._contractService = contractService;
            this._currencyService = currencyService;
            this._config = config;
            this._dataProvider = dataProvider;
            this._procuringAgencyService = procuringAgencyService;
            this._dbContext = dbContext;
            this._priceFormatter = priceFormatter;
        }
        #endregion
        #region task 
        public virtual IActionResult Index()
        {
            return View();
        }
        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTask))
                return AccessDeniedView();
            return View();
        }
        public virtual IActionResult _Create(int ContractId,int TaskId, int ParentTaskId=0, int AppendixId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            var item = new Task();
            if (TaskId > 0)
            {
                item = _workTaskService.GetTaskById(TaskId);               
            }
            var model = _taskModelFactory.PrepareTaskModel(
                new TaskModel { ContractId = ContractId,
                    Id = TaskId,
                    ParentId = ParentTaskId,
                    TaskGroupValue = _config.TaskGroupValueDefault,
                    TaxPercent = _config.TaxPercentDefault,
                    TaskGroupCost = _config.TaskGroupCostDefault,
                    TaskGroupSalary = _config.TaskGroupSalaryDefault
                }, item);
            model.AppendixId = AppendixId;
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _Create(TaskModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return JsonErrorMessage();
            if (ModelState.IsValid)
            {
                var note = "";
                var noti = "admin.common.Added";
                Task item = new Task();
                int PreParentTaskId = model.ParentId;
                if (model.Id > 0)
                {
                    item = _workTaskService.GetTaskById(model.Id);
                    PreParentTaskId = item.ParentId.GetValueOrDefault();
                    note =MessageReturn.CreateSuccessMessage("oldData",item.ToModel<TaskModel>()).toStringJson();
                }
                    _taskModelFactory.PrepareTask(item, model);
                if (model.Id > 0)
                {
                    _workTaskService.UpdateTask(item);
                    _taskModelFactory.InsertContractLog(item);
                    var TaskLogModel = new TaskLogModel
                    {
                        TaskId = item.Id,
                        Note = "Cập nhật công việc "
                    };
                    _taskModelFactory.InsertTaskLog(TaskLogModel);
                    noti = "admin.common.Updated";
                    //kiem tra xem co thay doi cha ko, neu co thay doi cha thi phai update lai cha
                    //if(PreParentTaskId!=model.ParentId && PreParentTaskId>0)
                    //{
                    //    _workTaskService.UpdateTaskAmount(PreParentTaskId);
                    //}
                }               
                else
                {
                    //neu them cong viec vao hop dong thi mac dinh cong viec o trang thai dang working
                    if(model.ContractId > 0)
                    {
                      var contract1 =  _contractService.GetContractById(model.ContractId);
                        if (contract1.StatusId != (int)ContractStatus.Draf)
                        {
                            item.StatusId = (int)TaskStatus.Working;
                        }
                    }
                    _workTaskService.InsertTask(item);
                    //lay lai thong tin da insert
                    item = _workTaskService.GetTaskById(item.Id);
                    _taskModelFactory.InsertContractLog(item);
                    var TaskLogModel = new TaskLogModel
                    {
                        TaskId = item.Id,
                        Note = "Thêm mới công việc "
                    };
                    _taskModelFactory.InsertTaskLog(TaskLogModel);
                }
                if (model.AppendixId > 0)
                {
                    var taskContract = new TaskContract
                    {
                        TaskId = item.Id,
                        ContractId = model.AppendixId,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        CreatedDate = DateTime.Now,
                        Note = note,
                    };
                    _workTaskService.InsertTaskContract(taskContract);
                }
                var contract = _contractService.GetContractById(item.ContractId);
                //neu hop dong dang chay va co ContractTypeId thì chay lai gt do dang
                if (contract != null && contract.StatusId != (int)ContractStatus.Draf && item.ContractTypeId >0 && item.StatusId != (int)TaskStatus.Draf)
                {
                    UpdateContractUnfinish(item.Id);
                }               
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<TaskModel>());
            }
            var list = ModelState.Values.Where(c=>c.Errors.Count >0).ToList();             
            return JsonErrorMessage("Error", list);
        }
        [HttpPost]
        public virtual IActionResult Create(TaskModel model)
        {
            return View(model);
        }      
        [HttpPost]
        public virtual IActionResult List(TaskSearchModel searchmodel)
        {            
            var items = _taskModelFactory.PrepareTaskListModel(searchmodel);
            return Json(items);
        }
        
        public virtual IActionResult _TaskDeadline(int ContractId)
        {
            var items = _workTaskService.GetTaskDeadLine(ContractId).Select(c => {
                var m = c.ToModel<TaskModel>();
                _taskModelFactory.PrepareDeadline(m, c);
                return m;
            }).ToList();

            var model = new TaskModel
            {
                taskDeadLineModel = items
            };

            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractTaskChild(TaskSearchModel searchmodel)
        {            
            var model = _taskModelFactory.PrepareTaskListModel(searchmodel);
            return PartialView(model);
        }       
        public virtual IActionResult Delete(int taskId)
        {           
            var task = _workTaskService.GetTaskById(taskId);            
            if(task != null)
            {
                //kiểm tra hạng mục con
               var taskchilds =  _workTaskService.getTasksByConTractId(ParentId: taskId);
                if (taskchilds.Count > 0)                
                    return JsonErrorMessage("Hạng mục có chứa hạng mục con !");
                //kiem tra co nghiem thu khoi luong hay chua
                var acceptancesubs = _contractService.GetallContractAcceptanceSubs(TaskId:taskId,TypeId:(int)ContractAcceptancesType.KhoiLuong);
                if (acceptancesubs.Count>0)
                {
                    return JsonErrorMessage("Hạng mục đã nghiệm thu khối lượng !");
                }
                //kiem tra da quyet toan hay chua 
                var settlementsubs = _contractService.GetAllContractSettlementSub(taskId: taskId);
                if (settlementsubs.Count > 0)
                {
                    return JsonErrorMessage("Hạng mục đã quyết toán !");
                }
                var model = _taskModelFactory.PrepareDelete(taskId);
                //xoa het cac mapping 
                _contractService.DeleteContractAcceptanceTaskMappingbyTaskId(taskId);
                var TaskLogModel = new TaskLogModel
                {
                    TaskId = taskId,
                    Note = "Xoá công việc "
                };
                _taskModelFactory.InsertTaskLog(TaskLogModel);
                var contract = _contractService.GetContractById(task.Id);
                //neu hop dong dang chay va co ContractTypeId thì chay lai gt do dang
                if (contract != null && contract.StatusId != (int)ContractStatus.Draf && task.ContractTypeId > 0)
                {
                    UpdateContractUnfinish(task.Id);
                }                
                return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), model);
            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        [HttpPost]
        public virtual IActionResult _GetTotalAmountTask(ContractPaymentPlanModel model)
        {
            if (model.PercentNum > 100)
            {
                model.ckPercentNumNote = "Tỷ lệ phần trăm vượt quá 100%.";
                return JsonSuccessMessage("ok", model);
            }
            if (model.SelectedTaskIds.Count > 0)
            {
                var items = _workTaskService.GetTaskbyListTaskIds(model.SelectedTaskIds.ToList());
                model.AmountSummary = "";
                var _currencies = items
                        .Select(t => new
                        {
                            t.CurrencyId,
                            t.TotalMoney
                        })
                        .GroupBy(g => g.CurrencyId)
                        .Select(x =>
                        {
                            var _contractCurrency = new ContractCurrency();
                            _contractCurrency.ContractId = model.ContractId;
                            _contractCurrency.CurrencyId = x.Key.GetValueOrDefault(0);
                            if (_contractCurrency.CurrencyId > 0)
                            {
                                _contractCurrency.currency = _currencyService.GetCurrencyById(_contractCurrency.CurrencyId);
                            }
                            _contractCurrency.TotalAmount = x.Sum(s => s.TotalMoney).GetValueOrDefault(0);
                            return _contractCurrency;
                        })
                        .OrderBy(o => o.currency.DisplayOrder)
                        .ToList();
                model.AmountSummary = String.Join(", ", _currencies.Select(c => _priceFormatter.FormatPrice(((c.TotalAmount * model.PercentNum) / 100), true, c.currency)));
                //model.AmountSummary = _currencies.ToAmountSummary(_currencySettings.PrimaryExchangeRateCurrencyId);
                //if (_currencies.Count > 1)
                //{
                //    int count = _currencies.Count;
                //    int i = 0;
                //    foreach (var currencie in _currencies)
                //    {
                //        i = i + 1;
                //        if (i == count)
                //        {
                //            model.AmountSummary = model.AmountSummary + ((currencie.TotalAmount * model.PercentNum) / 100).ToVNStringNumber() + " " + currencie.currency.CurrencyCode;
                //        }
                //        else
                //        {
                //            model.AmountSummary = model.AmountSummary + ((currencie.TotalAmount * model.PercentNum) / 100).ToVNStringNumber() + " " + currencie.currency.CurrencyCode + " ,";
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (var currencie in _currencies)
                //    {
                //        model.AmountSummary = model.AmountSummary + ((currencie.TotalAmount * model.PercentNum) / 100).ToVNStringNumber() + " " + currencie.currency.CurrencyCode;
                //    }
                //}
                return JsonSuccessMessage("ok", model);
            }
            else
            {
                model.AmountSummary = "";
                model.PercentNum = 0;
                return JsonSuccessMessage("ok", model);
            }
            
        }      
        public virtual IActionResult _GetTotalMoneyTask(int TaskID)
        {
            decimal money = 0;
            var items = _workTaskService.GetTaskById(TaskID);
            if (items != null)
            {
                money = (decimal)items.TotalMoney;
            }
            return JsonSuccessMessage("ok", money);
        }
        public virtual IActionResult CheckAmountTask(string totalMoney, int contractId)
        {
            var contract = _contractService.GetContractById(contractId);
            if (totalMoney.ToNumber() > contract.TotalAmount)
            {
                return JsonSuccessMessage("Số tiền đang nhập vượt quá số tiền của hợp đồng.", contract.ToModel<ContractModel>());
            }
            return JsonErrorMessage();
        }
        public virtual IActionResult _ProcuringAgencyTask(int contractId, int taskId)
        {
            var task = _workTaskService.GetTaskById(taskId);
            var model = task.ToModel<TaskModel>();
            model.Id = taskId;
            var listProcuringAgency = _contractService.GetallProcuringAgencyContract(contractId);
            model.lstProcuringAgency = listProcuringAgency.Where(c=>c.Id != (int)ProcuringAgencyId.Pecc1).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.TaskProcuringAgencyId,                
            }).ToList();
            model.lstProcuringAgency.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "---Chọn đơn vị liên danh---"
            });
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult ProcuringAgencyTaskAdd(TaskModel model)
        {   
            var task = _workTaskService.GetTaskById(model.Id);
            if (task != null)
            {
                if(model.TaskProcuringAgencyId > 0)
                {
                    task.TaskProcuringAgencyId = model.TaskProcuringAgencyId;
                }
                else
                {
                    task.TaskProcuringAgencyId = null;
                }
                _workTaskService.UpdateTask(task);
                return JsonSuccessMessage("Cập nhật liên danh thành công!");
            }
            //them contractlog vs Tasklog
            //Insert Contract Log
            var contract = _contractService.GetContractById(task.ContractId);
            var contractLog = new ContractLog()
            {
                ContractId = contract.Id,
                CreatedDate = DateTime.Now,
                CreatorId = _workContext.CurrentCustomer.Id,
                ContractData = task.ToModel<TaskModel>().toStringJson()
            };
            _contractLogService.InsertContractLog(contractLog, "Thêm liên danh cho công việc " + task.Name);
            // Insert TaskLog
            var TaskLogModel = new TaskLogModel
            {
                TaskId = task.Id,
                Note = "Thêm liên danh cho công việc."
            };
            _taskModelFactory.InsertTaskLog(TaskLogModel);
            return JsonErrorMessage();
        }
        #endregion
        #region TaskResource
        public virtual IActionResult _TaskResource(int TaskId)
        {
            var model = new TaskResourceModel
            {
                Id = TaskId
            };
            _workModelFactory.PrepareTaskResource(model);
            return PartialView(model);
        }
        public virtual IActionResult _TaskResourceAdd(int TaskId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //prepare model
            var modelview = new TaskResourceModel { TaskId = TaskId };
            _workModelFactory.PrepareTaskResourceEdit(modelview);
            return PartialView(modelview);
        }
        [HttpPost]
        public virtual IActionResult _TaskResourceUp(TaskResourceModel lsmodel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return JsonErrorMessage();         
           
            if (lsmodel.SelectedCustomerRoleIds.Count > 0)
            {
                _workTaskService.DeleteListTaskResource(lsmodel.TaskId, lsmodel.CustomerId);
            }
            foreach (int i in lsmodel.SelectedCustomerRoleIds)
            {
                TaskResource model = new TaskResource
                {
                    TaskId = lsmodel.TaskId,
                    CustomerId = lsmodel.CustomerId,
                    RoleId = i
                };
                _workTaskService.InsertTaskResource(model);
            }
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Works.Task.Resource.Updated"));
        }
        #endregion
        #region ContractUnfinish
        public void UpdateContractUnfinish(int TaskId)
        {
            var notiId = _dataProvider.GetParameter();
            notiId.ParameterName = "IdTask";
            notiId.Value = TaskId;
            notiId.DbType = DbType.Int32;
            _dbContext.ExecuteSqlCommand("EXEC [dbo].[SP_ContractUnfinishByTaskId] @IdTask ",
               false, 600, notiId);
        }
        #endregion
    }
}
