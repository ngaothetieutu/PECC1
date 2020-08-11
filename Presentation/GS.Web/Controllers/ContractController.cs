using GS.Core;
using GS.Core.Data;
using GS.Core.Domain.Common;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using GS.Data;
using GS.Services.Catalog;
using GS.Services.Configuration;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Factories;
using GS.Web.Framework.Controllers;
using GS.Web.Framework.Mvc.Filters;
using GS.Web.Models.Contracts;
using GS.Web.Models.Unit;
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GS.Web.Controllers
{
    public partial class ContractController : BaseWorksController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IContractService _contractService;
        private readonly IContractModelFactory _contractModelFactory;
        private readonly IWorkContext _workContext;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IContractFormService _contractFormService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IContractPeriodService _contractPeriodService;
        private readonly IContractLogService _contractLogService;
        private readonly IContractRelateService _contractRelateService;
        private readonly IContractPaymentService _contractPaymentService;
        private readonly IUnitService _unitService;
        private readonly ICurrencyService _currencyService;
        private readonly IWorkTaskService _workTaskService;
        private readonly ITaskModelFactory _taskModelFactory;
        private readonly IWorkTaskService _taskService;
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        #endregion

        #region Ctor

        public ContractController(IProcuringAgencyService procuringAgencyService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IWorkTaskService taskService,
            IUnitService unitService,
            IPriceFormatter priceFormatter,
            ICurrencyService currencyService,
            IContractLogService contractLogService,
            IContractPaymentService contractPaymentService,
            IContractTypeService contractTypeService,
            IContractRelateService contractRelateService,
            IContractFormService contractFormService,
            IContractPeriodService contractPeriodService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IContractModelFactory contractModelFactory,
            IWorkContext workContext,
            IContractService contractService,
            IWorkTaskService workTaskService,
             ITaskModelFactory taskModelFactory)
        {
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._taskService = taskService;
            this._currencyService = currencyService;
            this._contractTypeService = contractTypeService;
            this._contractRelateService = contractRelateService;
            this._unitService = unitService;
            this._contractLogService = contractLogService;
            this._contractPaymentService = contractPaymentService;
            this._contractFormService = contractFormService;
            this._contractPeriodService = contractPeriodService;
            this._procuringAgencyService = procuringAgencyService;
            this._contractModelFactory = contractModelFactory;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._contractService = contractService;
            this._workContext = workContext;
            this._workTaskService = workTaskService;
            this._taskModelFactory = taskModelFactory;
        }

        #endregion
        #region Methods Contract

        public virtual IActionResult List(string keySearch = null, int constructionTypeId = 0
            , int constructionId = 0, int classificationId = (int)ContractClassification.All, int StartYear = 0
            , int ContractMonitorStatus = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //prepare model
            ContractSearchModel searchModel = new ContractSearchModel
            {
                Keysearch = keySearch,
                ConstructionTypeId = constructionTypeId,
                ClassificationId = classificationId,
                StartYear = StartYear,
                contractMonitorStatus = ContractMonitorStatus,
            };
            if (constructionId > 0)
            {
                searchModel.SelectedConstructionIds.Add(constructionId);
            }
            var model = _contractModelFactory.PrepareContractSearchModel(searchModel);
            return View(model);
        }

        //[HttpPost]
        public virtual IActionResult _RecentlyContractViews(ContractSearchModel searchModel)
        {
            var contractviews = _contractService.getallContractView(_workContext.CurrentCustomer.Id);
            var model = contractviews.Select(c =>
            {
                var m = c.contract.ToModel<ContractModel>();
                m.DisplayType = searchModel.Display;

                _contractModelFactory.PrepareContractUnfinish(m);
                var contractMonitor = _contractService.GetContractMonitor(c.ContractId);
                if (contractMonitor != null)
                {
                    m.contractMonitor = contractMonitor.ToModel<ContractMonitorModel>();
                }

                //PrepareContractResource(m);
                return m;
            }).ToList();
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
            //    return AccessDeniedKendoGridJson();
            ////searchModel.PageSize = 4;
            //var model = _contractModelFactory.PrepareRecentlyContracts(searchModel);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _PartialList(ContractSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedKendoGridJson();
            //prepare model 
            var model = _contractModelFactory.PrepareContractListModel(searchModel);
            model.TypeDisplay = searchModel.TypeDisplay;
            model.CurrentPage = searchModel.Page;
            if (model.Total == 0)
            {
                model.CurrentPageTo = 0;
            }
            else
            {
                model.CurrentPageTo = (model.CurrentPage) * searchModel.PageSize + 1;
            }
            model.CurrentPageFrom = (model.CurrentPage) * searchModel.PageSize + model.Data.Count();

            return PartialView(model);
        }

        public virtual IActionResult Create(int ClassificationId = 1, int ContractRelateId = 0, int TaskId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //prepare model
            var model = new ContractModel
            {
                TaskIdBB = TaskId,
                ContractRelateId = ContractRelateId,
                ClassificationId = ClassificationId,
            };
            if (ClassificationId != (int)GS.Core.Domain.Contracts.ContractClassification.AB)
            {
                model.contractGuidAB = _contractService.GetContractById(ContractRelateId).ContractGuid;
            }
            model = _contractModelFactory.PrepareContractModel(model, null);
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var item = new Contract();
                _contractModelFactory.PrepareContract(item, model);
                //Update lai Hop Dong BB khi tao phu luc cho BB'
                if (model.ContractRelateId > 0)
                {
                    var contractRelateBB = _contractService.GetContractById(model.ContractRelateId);
                    if (contractRelateBB.classification == ContractClassification.BB)
                    {
                        contractRelateBB.TotalAmount = model.TotalAmountNumber.ToNumber();
                        _contractService.UpdateContract(contractRelateBB);
                    }
                }

                //ensure we have "/" at the end
                _contractService.InsertContract(item);
                if (model.ClassificationId == (int)ContractClassification.BB)
                {
                    if (model.Id > 0)
                    {
                        var TaskContract = _workTaskService.GetTaskContractByContractId(model.Id);
                        _workTaskService.deleteTaskContract(TaskContract);
                    }
                    if (model.TaskIdBB > 0)
                    {
                        var taskContract = new TaskContract
                        {
                            TaskId = model.TaskIdBB,
                            ContractId = item.Id,
                            CreatorId = _workContext.CurrentCustomer.Id,
                            CreatedDate = DateTime.Now,
                        };
                        _workTaskService.InsertTaskContract(taskContract);
                    }
                }
                //cap nhat thong tin hinh thuc hop dong
                //lay thong tin hinh thuc hop dong tu selectitem
                if (model.SelectedContractFormIds.Count > 0)
                {
                    var contractForms = _contractFormService.GetAllContractForms(model.SelectedContractFormIds);
                    foreach (var cf in contractForms)
                    {
                        item.ContractContractFormMappings.Add(new ContractContractFormMapping { contractForm = cf });
                    }
                }

                //lay thong tin loai hop dong tu slectitem
                if (model.SelectedContractTypeIds.Count > 0)
                {
                    var contractTypes = _contractTypeService.GetAllContractTypes(model.SelectedContractTypeIds);
                    foreach (var ct in contractTypes)
                    {
                        item.ContractContractTypeMappings.Add(new ContractContractTypeMapping { contractType = ct });
                    }
                }

                //lay thong tin giai doan hop dong tu slectitem
                if (model.SelectedContractPeriodIds.Count > 0)
                {
                    var contractPeriods = _contractPeriodService.GetAllContractPeriodsByListId(model.SelectedContractPeriodIds);
                    foreach (var cp in contractPeriods)
                    {
                        item.ContractContractPeriodMappings.Add(new ContractContractPeriodMapping { contractPeriod = cp });
                    }
                }

                _contractService.UpdateContract(item);
                //insert Contract Relate
                if (model.ContractRelateId > 0)
                {
                    var contractRelate = _contractModelFactory.PrepareContractRelate(item, model.ContractRelateId);
                    _contractRelateService.InsertContractRelate(contractRelate);
                    //luu contractLog
                    if (model.ContractRelateId > 0 && item.ClassificationId == 2)
                    {
                        var contractlog = _contractModelFactory.PrepareContractLog(item);
                        contractlog.ContractId = model.ContractRelateId;
                        _contractLogService.InsertContractLog(contractlog, "Tạo mới hợp đồng BB': " + item.Name);

                    }
                    else if (model.ContractRelateId > 0 && item.ClassificationId == 3)
                    {
                        var contractlog = _contractModelFactory.PrepareContractLog(item);
                        contractlog.ContractId = model.ContractRelateId;
                        _contractLogService.InsertContractLog(contractlog, "Tạo mới hợp đồng phụ lục: " + item.Name);
                    }
                    else
                    {
                        var contractlog = _contractModelFactory.PrepareContractLog(item);
                        _contractLogService.InsertContractLog(contractlog, "Tạo mới hợp đồng: " + item.Name);
                    }
                    if (item.ClassificationId == 3)
                    {
                        return RedirectToAction("EditAppendix", new { ContractId = model.ContractRelateId, AppendixId = contractRelate.Contract2Id }); ;
                    }
                }
                //activity log
                _customerActivityService.InsertActivity("AddNewContract",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContract"), item.Id), item);
                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Contract.Added"));
                return RedirectToAction("Detail", new { guid = item.ContractGuid });
            }

            //prepare model
            model = _contractModelFactory.PrepareContractModel(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult CheckCode1(string code1)
        {
            var item = _contractService.GetContractByCode1(code1);
            if (item != null)
            {
                var model = item.ToModel<ContractModel>();
                return JsonSuccessMessage("Số hợp đồng đã tồn tại :", model);
            }
            return JsonErrorMessage();
        }
        public virtual IActionResult EditContractCheckType(int ContractId, int ContractTypeId)
        {
            var item = _workTaskService.getTasksByConTractId(ContractId: ContractId, ContractTypeId: ContractTypeId);
            var contractType = _contractTypeService.GetContractTypeById(ContractTypeId);
            if (item.Count > 0)
            {
                return JsonSuccessMessage("Không thể bỏ loại hợp đồng " + contractType.Name + " vì đang tồn tại công việc thuộc loại hợp đồng này.");
            }
            return JsonErrorMessage();
        }
        public virtual IActionResult CheckAmountBB(string totalAmount, int taskId)
        {
            var task = _workTaskService.GetTaskById(taskId);
            if (totalAmount.ToNumber() > task.TotalMoney)
            {
                return JsonSuccessMessage("Số tiền đang nhập vượt quá số tiền của công việc.", task.ToModel<TaskModel>());
            }
            return JsonErrorMessage();
        }
        public virtual IActionResult EditAppendix(int ContractId = 0, int AppendixId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //prepare model
            var item = _contractService.GetContractById(ContractId);
            var model = _contractModelFactory.PrepareContractModel(null, item, true);
            //var model = item.ToModel<ContractModel>();
            model.AppendixId = AppendixId;
            return View(model);
        }
        public virtual IActionResult Detail(Guid guid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //try to get a store with the specified id

            var item = _contractService.GetContractByGuid(guid);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractModelFactory.PrepareContractModel(null, item, true);
            //add contractView
            ContractView ctv = new ContractView
            {
                ContractId = model.Id,
                CustomerId = _workContext.CurrentCustomer.Id
            };
            _contractService.InsertContractView(ctv);
            return View(model);
        }
        public virtual IActionResult IndexAppendix(Guid guid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            var item = _contractService.GetContractByGuid(guid);
            var model = new ContractAppendixModel();
            _contractModelFactory.PrepareIndexContractAppendix(item, model);
            return View(model);
        }
        public virtual IActionResult IndexAppendixEditPaymentPlan(Guid guid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            var item = _contractService.GetContractByGuid(guid);
            var model = new AppendixEditPaymenPlanModel();
            _contractModelFactory.PrepareIndexAppendixEditPaymentPlan(item, model);
            return View(model);
        }
        public virtual IActionResult Edit(Guid guid)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractService.GetContractByGuid(guid);

            if (item == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractModelFactory.PrepareContractModel(null, item);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ContractModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractService.GetContractById(model.Id);
            if (item == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                //lay ra danh sach Type Cu
                var oldTypes = item.ContractContractTypeMappings.Select(c => c.ContractTypeId).ToList();
                _contractModelFactory.PrepareContract(item, model);
                //xoa het contract form
                item.ContractContractFormMappings.Clear();
                _contractService.UpdateContract(item);
                item.ContractContractTypeMappings.Clear();
                _contractService.UpdateContract(item);
                item.ContractContractPeriodMappings.Clear();
                _contractService.UpdateContract(item);


                //cap nhat thong tin hinh thuc hop dong

                //lay thong tin hinh thuc hop dong tu selectitem
                if (model.SelectedContractFormIds.Count > 0)
                {
                    var contractForms = _contractFormService.GetAllContractForms(model.SelectedContractFormIds);
                    foreach (var cf in contractForms)
                    {
                        item.ContractContractFormMappings.Add(new ContractContractFormMapping { contractForm = cf });
                    }
                }
                //lay thong tin loai hop dong tu selectitem
                //if (model.SelectedContractTypeIds.Count > 0)
                //{
                var contractTypes = _contractTypeService.GetAllContractTypes(model.SelectedContractTypeIds);
                if (model.SelectedContractTypeIds.Count > 0)
                {
                    foreach (var ct in contractTypes)
                    {
                        item.ContractContractTypeMappings.Add(new ContractContractTypeMapping { contractType = ct });
                    }
                }
                var changeDeletes = oldTypes.Except(model.SelectedContractTypeIds).ToList();
                if (changeDeletes.Count > 0)
                {
                    foreach (int TypeId in changeDeletes)
                    {
                        UpdateContractUnfinishbyTypeId(item.Id, TypeId);
                    }
                }
                var changeNews = model.SelectedContractTypeIds.Except(oldTypes).ToList();
                if (changeNews.Count > 0)
                {
                    foreach (int TypeId in changeNews)
                    {
                        UpdateContractUnfinishbyTypeId(item.Id, TypeId);
                    }
                }
                //}
                //lay thong tin giai doan hop dong tu selectitem
                if (model.SelectedContractPeriodIds.Count > 0)
                {
                    var contractPeriods = _contractPeriodService.GetAllContractPeriodsByListId(model.SelectedContractPeriodIds);
                    foreach (var cp in contractPeriods)
                    {
                        item.ContractContractPeriodMappings.Add(new ContractContractPeriodMapping { contractPeriod = cp });
                    }
                }
                _contractService.UpdateContract(item);
                //luu contractLog
                var contractlog = _contractModelFactory.PrepareContractLog(item);
                _contractLogService.InsertContractLog(contractlog, "Cập nhật thông tin hợp đồng " + item.Name);
                //activity log
                _customerActivityService.InsertActivity("EditContract",
                    string.Format(_localizationService.GetResource("ActivityLog.EditContract"), item.Id), item);
                if (item.StatusId != (int)ContractStatus.Destroy && item.StatusId != (int)ContractStatus.Draf && item.ClassificationId == (int)ContractClassification.AB)
                {
                    UpdateContractUnfinish(item.Id);
                }
                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Contract.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("Detail", new { guid = item.ContractGuid });
            }

            //prepare model
            model = _contractModelFactory.PrepareContractModel(model, item, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractService.GetContractById(id);
            _contractService.DeleteContractView(id);
            if (item == null)
                return RedirectToAction("List");
            try
            {
                _contractService.DeleteContract(item);
                _contractService.DeleteContractUnfinishByContractId(item.Id);
                //luu contractLog
                var contractlog = _contractModelFactory.PrepareContractLog(item); ;
                _contractLogService.InsertContractLog(contractlog, "Xóa hợp đồng " + item.Name);
                //activity log
                _customerActivityService.InsertActivity("DeleteContract",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContract"), item.Id), item);
                SuccessNotification(_localizationService.GetResource("AppWork.Contracts.Contract.Deleted"));
                if (item.ClassificationId == (int)ContractClassification.AB)
                {
                    // Delete Contract Task
                    var _TasksContract = _workTaskService.GetAllTasks().Where( c => c.ContractId == item.Id);
                    foreach (var task in _TasksContract)
                    {
                        _workTaskService.DeleteTask(task);
                    }
                    var contractRelate = _contractRelateService.GetContractRelateByContract2Id(id);
                    if (contractRelate != null)
                    {
                        var contract = _contractService.GetContractById(contractRelate.Contract1Id);
                        return RedirectToAction("Detail", new { guid = contract.ContractGuid });
                    }
                }
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = item.Id });
            }
        }
        public virtual IActionResult _ListContractAppendix(int ContractId)
        {
            var model = new ContractListModel();
            var items = _contractService.GetContractRelates(ContractId, ContractClassification.Appendix);
            model.Data = items.Select(c => c.ToModel<ContractModel>());
            return PartialView(model);
        }
        public virtual IActionResult _ContractLiquidation(int ContractId)
        {
            var model = new ContractLiquidationModel
            {
                ContractId = ContractId
            };
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractLiquidation(ContractLiquidationModel model)
        {
            var contract = _contractService.GetContractById(model.ContractId);
            if (contract != null)
            {
                contract.StatusId = (int)ContractStatus.Liquidated;
                _contractService.UpdateContract(contract);
                if (model.SelectedListFileId.Count > 0)
                {
                    foreach (int FileId in model.SelectedListFileId)
                    {
                        var contractFile = new ContractFile
                        {
                            ContractId = model.ContractId,
                            FileId = FileId,
                            TypeId = (int)ContractFileType.LiquidationContract,
                        };
                        _contractService.InsertContractFile(contractFile);
                    }
                }
                var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                _contractLogService.InsertContractLog(contractlog, "Thanh lý hợp đồng " + contract.Name);
                return JsonSuccessMessage("Thanh lý thành công");
            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        public virtual IActionResult _ContractFinish(int contractId)
        {
            var model = new ContractModel
            {
                Id = contractId
            };
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractFinish(ContractModel model)
        {
            var contract = _contractService.GetContractById(model.Id);
            if (contract != null)
            {
                contract.StatusId = (int)ContractStatus.Completed;
                contract.FinishedDate = model.FinishedDate;
                _contractService.UpdateContract(contract);
                var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                _contractLogService.InsertContractLog(contractlog, "Kết thúc hợp đồng " + contract.Name);
                return JsonSuccessMessage("Kết thúc hợp đồng thành công");
            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        /// <summary>
        /// chuyen HD thanh HD chinh thuc
        /// </summary>
        /// <param name="ContractId"></param>
        /// <returns></returns>
        public virtual IActionResult _ContractApprove(int ContractId)
        {
            var contract = _contractService.GetContractById(ContractId);
            if (contract != null)
            {
                contract.StatusId = (int)ContractStatus.Building;
                _contractService.UpdateContract(contract);
                //duyet hop dong BB
                var ContractBBs = _contractService.GetContractRelates(ContractId, ContractClassification.BB);
                foreach (Contract ct in ContractBBs)
                {
                    UpdateContractStatus(ct, (int)ContractStatus.Building);
                }
                //duyet hop dong phu luc
                var ContractAppendix = _contractService.GetContractRelates(ContractId, ContractClassification.Appendix);
                foreach (Contract ct in ContractAppendix)
                {
                    UpdateContractStatus(ct, (int)ContractStatus.Building);
                }
                var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                //duyet cac cong viec sang trang thai hoat dong
                var tasks = _workTaskService.getTasksByConTractId(ContractId: ContractId);
                foreach (Task task in tasks)
                {
                    task.StatusId = (int)TaskStatus.Working;
                    _workTaskService.UpdateTask(task);
                    var TaskLogModel = new TaskLogModel
                    {
                        TaskId = task.Id,
                        Note = "Duyệt công việc "
                    };
                    _taskModelFactory.InsertTaskLog(TaskLogModel);
                }
                _contractLogService.InsertContractLog(contractlog, "Duyệt hợp đồng " + contract.Name);
                UpdateContractUnfinish(ContractId);
                return JsonSuccessMessage("Hợp đồng đã được duyệt");
            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        public virtual IActionResult _RestoreContract(int ContractId)
        {
            var contract = _contractService.GetContractById(ContractId);
            if (contract != null)
            {
                contract.StatusId = (int)ContractStatus.Draf;
                contract.Code1 = contract.Code1.Replace("-deleted", "");
                contract.Code = contract.Code.Replace("-deleted", "");
                _contractService.UpdateContract(contract);
                if (contract.ClassificationId == (int)ContractClassification.AB)
                {
                    // khoi phuc lai list task cua hop dong
                    var lstTask = _workTaskService.GetAllTasksByContractDestroyId(ContractId);
                    foreach (var task in lstTask)
                    {
                        task.StatusId = (int)TaskStatus.Draf;
                        _workTaskService.UpdateTask(task);
                    }
                }
                return JsonSuccessMessage("Khôi phục thành công ");
            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        public virtual IActionResult _CancelApprovalContract(int ContractId) {
            var contract = _contractService.GetContractById(ContractId);
            if (contract != null)
            {
                // kiem tra neu hop dong co NTKL thi ko the huy  duyet
                var AcceptanceKL= _contractService.getAllContractAcceptance(ContractId: ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
                if (AcceptanceKL.Count > 0)
                {
                    return JsonErrorMessage("Hợp đồng có NTKL, không thể hủy duyệt.");
                }
                // kiem tra neu hop dong co Quyet toan thi ko the huy  duyet
                var ContractSetlements = _contractService.getAllContractSettlement(ContractId);
                if (ContractSetlements.Count > 0)
                {
                    return JsonErrorMessage("Hợp đồng có quyết toán, không thể hủy duyệt.");
                }
                // kiem tra neu hop dong co "Tien do thanh toan" thi ko the huy  duyet
                var ContractPaymentPlans = _contractService.GetAllContractPaymentPlans(ContractId);
                if (ContractPaymentPlans.Count > 0)
                {
                    return JsonErrorMessage("Hợp đồng có tiến độ thanh toán, không thể hủy duyệt.");
                }
                UpdateContractStatus(contract, (int)ContractStatus.Draf);
                //thay doi hang muc cong viec ve draf
                var tasks = _workTaskService.getTasksByConTractId(ContractId: ContractId);
                foreach (Task task in tasks)
                {
                    task.StatusId = (int)TaskStatus.Draf;
                    _workTaskService.UpdateTask(task);
                    var TaskLogModel = new TaskLogModel
                    {
                        TaskId = task.Id,
                        Note = "Huỷ duyệt công việc "
                    };
                    _taskModelFactory.InsertTaskLog(TaskLogModel);
                }
                //xoa het gia tri do dang
                _contractService.DeleteContractUnfinishByContractId(contract.Id);
                //Insert  Contract Log
                var contractLog = new ContractLog()
                {
                    ContractId = contract.Id,
                    CreatedDate = DateTime.Now,
                    CreatorId = _workContext.CurrentCustomer.Id,
                    ContractData = contract.ToModel<ContractModel>().toStringJson()
                };
                _contractLogService.InsertContractLog(contractLog, "Hủy duyệt hợp đồng " + contract.Name);
                return JsonSuccessMessage("Hủy duyệt hợp đồng thành công.");

            }
            return JsonErrorMessage("Lỗi hệ thống");
        }
        public void UpdateContractStatus(Contract contract, int status)
        {
            contract.StatusId = status;
            _contractService.UpdateContract(contract);
        }
        public virtual IActionResult _ContractAppendixList(int TaskId = 0, int paymentPlanId = 0)
        {
            var model = new ContractListModel();
            if (TaskId > 0)
            {
                var task = _workTaskService.GetTaskById(TaskId);
                if (task != null)
                {
                    model.Data = task.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.Appendix).Select(x => x.contract.ToModel<ContractModel>()).ToList();
                }
            }
            if (paymentPlanId > 0)
            {
                var _paymentPlanContracts = _contractService.GetPaymentPlanContractsByPaymentPlanId(paymentPlanId);
                if (_paymentPlanContracts != null)
                {
                    var paymentPlanContractIds = _paymentPlanContracts.Select(c => c.ContractId).ToList();
                    var lstContractAppendix = _contractService.GetListContractbylistId(paymentPlanContractIds);
                    model.Data = lstContractAppendix.Select(c => c.ToModel<ContractModel>()).ToList();
                }
            }
            return PartialView(model);
        }
        public virtual IActionResult _ContractAppendixEditPaymentPlanList(int paymentPlanId = 0)
        {
            var model = new ContractListModel();
            if (paymentPlanId > 0)
            {
                var _paymentPlanContracts = _contractService.GetPaymentPlanContractsByPaymentPlanId(paymentPlanId);
                if (_paymentPlanContracts != null)
                {
                    var paymentPlanContractIds = _paymentPlanContracts.Select(c => c.ContractId).ToList();
                    var lstContractAppendix = _contractService.GetListContractbylistId(paymentPlanContractIds);
                    model.Data = lstContractAppendix.Select(c => c.ToModel<ContractModel>()).ToList();
                }
            }
            return PartialView(model);
        }
        public virtual IActionResult _ContractBBList(int TaskId)
        {
            var task = _workTaskService.GetTaskById(TaskId);
            var model = new ContractListModel();
            if (task != null)
            {
                model.Data = task.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract.ToModel<ContractModel>()).ToList();
            }
            return PartialView(model);
        }
        public virtual IActionResult _PaymentPlan(int contractId)
        {
            var model = _contractService.GetPaymentPlan(contractId);
            return PartialView(model);
        }
        public virtual IActionResult _ContractAdvanceQuantityList(int ContractId, int TaskId = 0)
        {
            var searchmodel = new ContractAcceptanceSearchModel
            {
                TaskId = TaskId,
                ContractId = ContractId,
                TypeId = (int)ContractAcceptancesType.TamUng,
            };
            var model = _contractModelFactory.PrepareContractAcceptanceListModel(searchmodel);
            return PartialView(model);
        }
        public virtual IActionResult _StatisticalConstructionType(int constructionType, int StartYear)
        {
            var model = _contractService.GetTotalTypeContractByYear(StartYear, constructionType).ToList();
            return PartialView(model);
        }
        #endregion
        #region Thong tin chu dau tu


        // Partial view Hoat Dong
        //public virtual IActionResult _TaskActivity(int contractId)
        //{
        //    var model = new ContractModel();
        //    model.Id = contractId;
        //    _contractModelFactory.PrepareContractLogActivity(model);
        //    return PartialView(model);
        //}

        public virtual IActionResult SearchProcuringAgency(string Name)
        {
            var items = _procuringAgencyService.GetAllProcuringAgencys(Name);
            return Json(items.Select(c =>
            {
                var m = c.ToModel<ProcuringAgencyModel>();
                return m;
            }
            ).ToList());
        }
        public virtual IActionResult SearchContractForm(string Name)
        {
            var items = _contractFormService.GetAllContractFormsByName(Name);
            return Json(items.Select(c =>
            {
                var m = c.ToModel<ContractFormModel>();
                return m;
            }
            ).ToList());
        }
        public virtual IActionResult SearchContractType(string Name)
        {
            var items = _contractTypeService.GetAllContractTypeByName(Name);
            return Json(items.Select(c =>
            {
                var m = c.ToModel<ContractTypeModel>();
                return m;
            }
            ).ToList());
        }
        #endregion
        #region ContractJoint
        public virtual IActionResult _ContractJointInfo(int contractId)
        {
            var model = new ContractModel();
            model.Id = contractId;
            _contractModelFactory.PrepareprocuringAgencyName(model);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult InsertContractJoint(ContractJointModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            var ck = _contractService.GetContractJointById(model.ContractId, model.ProcuringAgencyId);
            if (ck == null)
            {
                var item = new ContractJoint();
                if (item != null)
                {
                    _contractModelFactory.PrepareContractJoint(item, model);
                    _contractService.InsertContractJoint(item);
                    //luu contractLog
                    var procuring = _procuringAgencyService.GetProcuringAgencyById(item.ProcuringAgencyId);
                    var contract = _contractService.GetContractById(item.ContractId);
                    var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                    _contractLogService.InsertContractLog(contractlog, "Thêm mới chủ đầu tư " + procuring.Name + " vào hợp đồng " + contract.Name);
                    //activity log
                    _customerActivityService.InsertActivity("AddNewContract",
                        string.Format(_localizationService.GetResource("ActivityLog.AddNewContractJoint"), item.Id), item);
                    return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewContractJoint"));
                }

            }
            //if ()
            //{

            //}
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.Contract.AddNewContractJointError"));

        }
        public ActionResult DeleteContractJoint(int _ContractId, int _ProCuringAgencyId)
        {
            var task = _workTaskService.GetTaskByTaskProcuringAgencyId(_ProCuringAgencyId);
            if (task != null)
            {
                return JsonErrorMessage("Không thể xóa chủ đầu tư.");
            }
            _contractService.DeleteContractJoint(_ContractId, _ProCuringAgencyId);
            //luu contractLog
            var procuring = _procuringAgencyService.GetProcuringAgencyById(_ProCuringAgencyId);
            var contract = _contractService.GetContractById(_ContractId);
            var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
            _contractLogService.InsertContractLog(contractlog, "Bỏ chủ đầu tư " + procuring.Name + " khỏi hợp đồng " + contract.Name);
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.DeleteContractJoint"));
        }
        #endregion
        #region Contract file
        public virtual IActionResult _ContractFiles(Guid contractGuid)
        {
            var model = new ContractModel();
            model.ContractGuid = contractGuid;
            model.workFileModels = _contractService.GetContractFiles(contractGuid).Select(c => c.ToModel<WorkFileModel>()).ToList();
            return PartialView(model);
        }
        #endregion
        #region Contract Payment
        public virtual IActionResult _ContractPaymentBBAdd(int ContractId, int TaskId = 0, int Id = 0)
        {
            var model = new ContractPaymentModel()
            {
                ContractId = ContractId,
                TaskId = TaskId
            };
            if (Id > 0)
            {
                var item = _contractPaymentService.GetContractPaymentById(Id);
                //prepare model
                model = _contractModelFactory.PrepareContractPaymentBBModel(null, item);
                return PartialView(model);
            }
            else
            {
                //prepare model
                model = _contractModelFactory.PrepareContractPaymentBBModel(model, null);
            }
            return PartialView(model);
        }
        public virtual IActionResult _InsertPayment(int ContractId, bool IsReceipts = true, int TypeId = (int)ContractPaymentType.Payment, int PaymentRequestId = 0, int Id = 0, int AcceptanceId = 0, int TaskId = 0)
        {
            var model = new ContractPaymentModel()
            {
                ContractId = ContractId,
                IsReceipts = IsReceipts,
                TypeId = TypeId,
                PaymentRequestId = PaymentRequestId,
                AcceptanceId = AcceptanceId,
                TaskId = TaskId
            };
            if (Id > 0)
            {
                var item = _contractPaymentService.GetContractPaymentById(Id);
                //prepare model
                model = _contractModelFactory.PrepareContractPaymentModel(null, item);
                return PartialView(model);
            }
            else
            {
                //prepare model
                model = _contractModelFactory.PrepareContractPaymentModel(model, null);
            }
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _InsertPayment(ContractPaymentModel model)
        {
            if (ModelState.IsValid)
            {
                var noti = "admin.common.Added";
                ContractPayment item = new ContractPayment();
                if (model.Id > 0)
                {
                    item = _contractPaymentService.GetContractPaymentById(model.Id);
                }
                _contractModelFactory.PrepareContractPayment(item, model);
                if (model.Id == 0)
                {
                    _contractPaymentService.InsertContractPayment(item);
                    if (model.AcceptanceId > 0)
                    {
                        var contractPaymentTask = new ContractPaymentTask()
                        {
                            PaymentId = item.Id,
                        };
                        var contractAcceptanceTask = _contractService.GetContractAcceptanceTaskMappingByAcceptanceId((int)item.AcceptanceId);
                        contractPaymentTask.TaskId = contractAcceptanceTask.TaskId;
                        contractPaymentTask.Amount = item.Amount;
                        contractPaymentTask.CurrencyId = item.CurrencyId;
                        _contractService.InsertContractPaymentTask(contractPaymentTask);
                    }
                }
                else _contractPaymentService.UpdateContractPayment(item);
                //luu contract Log
                var contract = _contractService.GetContractById(model.ContractId);
                var contractlog = _contractModelFactory.PrepareContractLog(contract);
                if (model.Id == 0)
                {
                    _contractLogService.InsertContractLog(contractlog, "Thêm mới thanh toán " + model.Code + " vào hợp đồng " + contract.Name);
                }
                else
                {
                    _contractLogService.InsertContractLog(contractlog, "Cập nhật thanh toán " + model.Code + " của hợp đồng " + contract.Name);
                    noti = "admin.common.Updated";
                }
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<ContractPaymentModel>());
            }
            else
            {
                var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
                return JsonErrorMessage("Error", list);
            }
        }
        [HttpPost]
        public virtual IActionResult _InsertPaymentBB(ContractPaymentModel model)
        {
            if (ModelState.IsValid)
            {
                var noti = "admin.common.Added";
                ContractPayment item = new ContractPayment();
                if (model.Id > 0)
                {
                    item = _contractPaymentService.GetContractPaymentById(model.Id);
                }
                _contractModelFactory.PrepareContractPaymentBB(item, model);
                if (model.Id == 0)
                {
                    _contractPaymentService.InsertContractPayment(item);
                }
                else
                {
                    _contractPaymentService.UpdateContractPayment(item);
                    noti = "admin.common.Updated";
                }
                //luu contract Log
                var contract = _contractService.GetContractById(model.ContractId);
                var contractlog = _contractModelFactory.PrepareContractLog(contract);
                if (model.Id == 0)
                {
                    _contractLogService.InsertContractLog(contractlog, "Thêm mới thanh toán BB" + model.Code + " vào hợp đồng " + contract.Name);
                }
                else
                {
                    _contractLogService.InsertContractLog(contractlog, "Cập nhật thanh toán " + model.Code + " của hợp đồng " + contract.Name);

                }
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<ContractPaymentModel>());
            }
            else
            {
                var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
                return JsonErrorMessage("Error", list);
            }
        }
        public virtual IActionResult _ContractPaymentList(int contractId)
        {
            var model = new ContractPaymentSearchModel { ContractId = contractId };
            var items = _contractModelFactory.PrepareContractPaymentSearchModel(model);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _TasksPayment(ContractPaymentSearchModel searchmodel)
        {
            var model = _contractModelFactory.PrepareContractPaymentListModel(searchmodel);
            return Json(model);
        }
        public virtual IActionResult SearchUnit(string Name)
        {
            var items = _unitService.GetAllUnits(Name);
            return Json(items.Select(c =>
            {
                var m = c.ToModel<UnitModel>();
                return m;
            }
            ).ToList());
        }
        [HttpPost]
        public virtual IActionResult DeleteContractPayment(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractPaymentService.GetContractPaymentById(id);
            if (item == null)
                return RedirectToAction("List");
            try
            {
                _contractPaymentService.DeleteContractPayment(item);
                //luu contractLog
                //var contract = _contractService.GetContractById();
                //var contractlog = _contractModelFactory.PrepareContractLog(item); ;
                //_contractLogService.InsertContractLog(contractlog, "Xóa hợp đồng " + item.Name);
                //activity log
                _customerActivityService.InsertActivity("DeleteContractPayment",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContractPayment"), item.Id), item);

                return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contractpayment.Deleted"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = item.Id });
            }
        }
        public virtual IActionResult _getCurrencyValue(int id)
        {
            var item = _currencyService.GetCurrencyById(id);
            if (item == null)
            {
                return JsonErrorMessage("Error", item);
            }
            return JsonSuccessMessage("Ok", item);
        }

        #endregion
        #region Contract Resource
        public virtual IActionResult _ContractResource(int ContractId)
        {
            var model = new ContractModel
            {
                Id = ContractId
            };
            _contractModelFactory.PrepareContractResource(model);
            return PartialView(model);
        }
        public virtual IActionResult _ContractResourceAdd(ContractResourceModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();
            //prepare model
            //var modelview = new ContractResourceModel {ContractId = model.ContractId, CustomerId = model.CustomerId};
            _contractModelFactory.prepareContractResourceModel(model);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractResourcUpd(ContractResourceModel lsmodel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return JsonErrorMessage();
            if (lsmodel.SelectedCustomerRoleIds.Count > 0)
            {
                _contractService.DeleteListContractResource(lsmodel.ContractId, lsmodel.CustomerId);
            }
            foreach (int i in lsmodel.SelectedCustomerRoleIds)
            {
                ContractResource model = new ContractResource
                {
                    ContractId = lsmodel.ContractId,
                    CustomerId = lsmodel.CustomerId,
                    RoleId = i
                };
                _contractService.InsertContractResource(model);
            }
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.Resource.Updated"));
        }
        public virtual IActionResult DeleteContractResoure(int ContractId, int customerId)
        {
            var lstcontractResoure = _contractService.GetContractResources(ContractId: ContractId, CustomerId: customerId);
            foreach (var contractResoure in lstcontractResoure)
            {
                _contractService.DeleteContractResource(contractResoure.Id);
            }
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.Contract.Resource.Delete"));
        }
        /// <summary>
        /// Lay thong tin resource theo contract Id, du lieu tra ve dang json
        /// </summary>
        /// <param name="ContractId"></param>
        /// <returns></returns>
        public virtual IActionResult _ContractResources(int ContractId)
        {
            var model = new ContractModel
            {
                Id = ContractId
            };
            _contractModelFactory.PrepareContractResource(model);
            return Json(model.contractResourceModels);
        }
        #endregion
        #region ContractLog
        public virtual IActionResult _ContractLogList(int contractId)
        {
            var model = new ContractLogSearchModel { ContractId = contractId };
            //var items = _contractModelFactory.PrepareContractPaymentListModel(model);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ListContractLog(ContractLogSearchModel searchmodel)
        {
            var model = _contractModelFactory.PrepareContractLogListModel(searchmodel);
            return Json(model);
        }
        #endregion 
        #region Contract PaymentPlan
        public virtual IActionResult _ContractPaymentPlan(int ContractId)
        {
            var searchmodel = new ContractPaymentPlanSearchModel
            {
                ContractId = ContractId,
            };
            var items = _contractModelFactory.PrepareContractPaymentPlanSearch(searchmodel);
            var model = new ContractModel
            {
                lstContractPaymentPlan = items.Data.ToList(),
            };
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractPaymentPlan(ContractPaymentPlanSearchModel searchmodel)
        {
            var model = _contractModelFactory.PrepareContractPaymentPlanSearch(searchmodel);
            return Json(model);
        }
        public virtual IActionResult _ContractPaymentPlanAdd(int ContractId, int PaymentPlanId, int TypeId = 1, int appendixId = 0)
        {
            var model = new ContractPaymentPlanModel
            {
                ContractId = ContractId,
                Id = PaymentPlanId,
                TypeId = TypeId
            };
            if (model.Id > 0)
            {
                model = _contractService.GetContractPaymentPlanById(model.Id).ToModel<ContractPaymentPlanModel>();
                model.AppendixId = appendixId;
            }
            _contractModelFactory.prepareContractPaymentPlanModel(model, null);

            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractPaymentPlanAdd(ContractPaymentPlanModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedTaskIds != null)
                {
                    ContractPaymentPlanRule rule = new ContractPaymentPlanRule
                    {
                        lstTaskId = model.SelectedTaskIds,
                    };
                    model.PayRule = rule.toStringJson();
                }
                //model.PayRule = rule
                var item = new ContractPaymentPlan();
                if (model.Id > 0)
                {
                    item = _contractService.GetContractPaymentPlanById(model.Id);
                    if (model.AppendixId > 0)
                    {
                        var paymentPlanContract = new PaymentPlanContract();
                        paymentPlanContract.ContractId = model.AppendixId;
                        paymentPlanContract.PaymentPlanId = model.Id;
                        paymentPlanContract.CreateDate = DateTime.Now;
                        paymentPlanContract.Note = MessageReturn.CreateSuccessMessage("oldData", item.ToModel<ContractPaymentPlanModel>()).toStringJson();
                        _contractService.InsertPaymentPlanContract(paymentPlanContract);
                    }
                }
                _contractModelFactory.prepareContractPaymentPlan(model, item);

                if (model.Id > 0)
                {
                    _contractService.UpdateContractPaymentPlan(item);
                    return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentPlan.Update"));
                }
                else
                {
                    _contractService.InsertContractPaymentPlan(item);
                    return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentPlan.Add"));
                }
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);

        }
        public virtual IActionResult DeleteContractPaymentPlan(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractService.GetContractPaymentPlanById(id);
            var contract = _contractService.GetContractById(item.ContractId);
            if (item == null)
                return RedirectToAction("Detail", new { guid = contract.ContractGuid });
            try
            {
                _contractService.DeleteContractPaymentPlan(item);
                //luu contractLog
                var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                _contractLogService.InsertContractLog(contractlog, "Xóa thanh toán " + item.Description);
                //activity log
                _customerActivityService.InsertActivity("DeleteContract",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContract"), item.Id), item);
                return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Error"));
            }
        }

        public virtual IActionResult _Unfinished(int contractId)
        {
            var lstTask = _workTaskService.getTasksByConTractId(ContractId: contractId, ParentId: 0);
            var model = _contractService.GetContractById(contractId).ToModel<ContractModel>();
            model.LstTaskModel = lstTask.Select(c => _taskModelFactory.PrepareTaskModelList(c)).ToList();
            model.LstTaskModelChuaThucHien = model.LstTaskModel.Where(c => c.StartDate.AddSeconds(-1) > DateTime.Now).ToList();
            model.LstTaskModelDangThucHien = model.LstTaskModel.Where(c => c.StartDate.AddSeconds(-1) <= DateTime.Now).ToList();
            ///so ngay cua hop dong
            //var intervalDaysContract = model.EndDate.Subtract(model.StartDate).Days + 1;            
            //var interval = DateTime.Now.Subtract(model.StartDate);
            /////so ngay den hien tai            
            //var intervalDaysPerformContract = interval.Days;
            //long spandate = (intervalDaysPerformContract / intervalDaysContract);
            //if (spandate>1)
            //{
            //    spandate = 1;
            //}
            //var lstTask = _workTaskService.GetAllTaskByContractIdAndStartdate(contractId);
            //var TotalAmountAccepted = _contractService.GetTotalAmountAcceptanced(contractId);
            model.Unfinished1 = model.LstTaskModelDangThucHien.Sum(c => c.Unfinished).ToVNStringNumber();
            model.Unfinished2 = model.LstTaskModelChuaThucHien.Sum(c => c.Unfinished).ToVNStringNumber();
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _Unfinished(ContractModel model)
        {
            var contract = _contractService.GetContractById(model.Id);
            contract.Unfinished1 = model.Unfinished1.ToNumber();
            contract.Unfinished2 = model.Unfinished2.ToNumber();
            _contractService.UpdateContract(contract);
            return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.AddUnfinish"), model);
        }
        public virtual IActionResult _ContractUnfinish(int contractId, int ContractTypeId)
        {
            var model = new ContractUnfinishSearchModel
            {
                ContractId = contractId,
                ContractTypeId = ContractTypeId,
                DaySearch = DateTime.Now,
            };
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractUnfinishSearch(ContractUnfinishSearchModel Searchmodel)
        {
            if (Searchmodel.DaySearch == null)
                Searchmodel.DaySearch = DateTime.Now;
            var model = _contractService.getallContractUnfinish(ContractId: Searchmodel.ContractId, ContractTypeId: Searchmodel.ContractTypeId, CreatedDate: Searchmodel.DaySearch).Select(c => c.ToModel<ContractUnfinishModel>()).FirstOrDefault();
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult ContractUnfinishEdit(ContractUnfinishModel model)
        {
            var item = _contractService.getContractUnfinishById(model.Id);
            if (item == null)
                return JsonErrorMessage("Xuất hiện lỗi");
            item.OptionValue = model.OptionValue.ToNumber();
            _contractService.UpdateContractUnfinish(item);
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Updated"));
        }
        #endregion
        #region ContractPaymentRequest
        public virtual IActionResult _ContractPaymentRequestAdd(int PaymentPlanId, int PaymentRequestId = 0, int TypeId = 0)
        {
            var model = new ContractPaymentRequestModel();
            model.Id = PaymentRequestId;
            model.TypeId = TypeId;
            if (model.Id > 0)
            {
                model = _contractService.GetContractPaymentRequestById(model.Id).ToModel<ContractPaymentRequestModel>();
            }
            _contractModelFactory.prepareContractPaymentRequestModelAdd(model, PaymentPlanId);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractPaymentRequestAdd(ContractPaymentRequestModel model)
        {
            if (ModelState.IsValid)
            {

                var item = new ContractPaymentRequest();
                _contractModelFactory.prepareContractPaymentRequest(model, item);

                if (item != null)
                {
                    if (model.Id > 0)
                    {
                        _contractService.UpdateContractPaymentRequest(item);

                        if (model.SelectedContractPaymentAcceptence.Count > 0)
                        {
                            var lstContractPaymentAcceptance = _contractService.GetAllContractPaymenAcceptanceByPaymentRequestId(item.Id);
                            foreach (var ContractPaymentAcceptance in lstContractPaymentAcceptance)
                            {
                                ContractPaymentAcceptance.PaymentRequestId = null;
                                _contractService.UpdateContractPaymentAcceptance(ContractPaymentAcceptance);
                            }
                            foreach (var PaymentAcceptenceId in model.SelectedContractPaymentAcceptence)
                            {
                                var PaymentAcceptence = _contractService.getContractPaymentAcceptancebyId(PaymentAcceptenceId);
                                PaymentAcceptence.PaymentRequestId = item.Id;
                                _contractService.UpdateContractPaymentAcceptance(PaymentAcceptence);
                            }
                        }
                        return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Update"));
                    }
                    else
                    {
                        _contractService.InsertContractPaymentRequest(item);
                        if (model.SelectedContractPaymentAcceptence.Count > 0)
                        {

                            foreach (var PaymentAcceptenceId in model.SelectedContractPaymentAcceptence)
                            {
                                var PaymentAcceptence = _contractService.getContractPaymentAcceptancebyId(PaymentAcceptenceId);
                                PaymentAcceptence.PaymentRequestId = item.Id;
                                _contractService.UpdateContractPaymentAcceptance(PaymentAcceptence);
                            }
                        }
                        return JsonSuccessMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Add"));
                    }
                }
            }
            else
            {
                var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
                return JsonErrorMessage("Error", list);
            }
            return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Add"));
        }

        public virtual IActionResult _ContractPaymentRequest(int ContractId, int paymentPlanId)
        {
            var model = new ContractPaymentRequestListModel();
            var _paymentPlan = _contractService.GetContractPaymentPlanById(paymentPlanId);
            var items = _contractService.GetAllContractPaymentRequestByContractIdAndPlanId(ContractId, paymentPlanId);
            model.Data = items.Select(c => _contractModelFactory.PrepareContractPaymentRequestModel(null, c)).ToList();
            model.PaymentPlanId = paymentPlanId;
            model.TypeId = _paymentPlan.TypeId;
            return PartialView(model);
        }
        public virtual IActionResult _ListContractPayment(int paymentRequestId = 0, int acceptanceId = 0)
        {
            var model = new ContractPaymentListModel();
            if (paymentRequestId > 0)
            {
                var items = _contractPaymentService.GetAllContractPaymentsByPaymentRequestId(paymentRequestId);
                model.Data = items.Select(c => c.ToModel<ContractPaymentModel>()).ToList();
            }
            if (acceptanceId > 0)
            {
                var items = _contractPaymentService.GetAllContractPaymentsByAcceptanceId(acceptanceId);
                model.Data = items.Select(c => c.ToModel<ContractPaymentModel>()).ToList();
            }
            return PartialView(model);
        }

        public virtual IActionResult DeleteContractPaymentRequest(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageContract))
                return AccessDeniedView();

            //try to get a store with the specified id
            var item = _contractService.GetContractPaymentRequestById(id);
            var contract = _contractService.GetContractById(item.ContractId);
            if (item == null)
                return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Error"));
            try
            {
                _contractService.DeleteContractPaymentRequest(item);
                //luu contractLog
                var contractlog = _contractModelFactory.PrepareContractLog(contract); ;
                _contractLogService.InsertContractLog(contractlog, "Xóa đề nghị thanh toán " + item.Name);
                //activity log
                _customerActivityService.InsertActivity("DeleteContract",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteContract"), item.Id), item);
                return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return JsonErrorMessage(_localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Error"));
            }
        }
        #endregion
        #region ContractAcceptance
        public virtual IActionResult _ContractAcceptance(int ContractId, int TypeId)
        {
            var items = _contractService.getAllContractAcceptance(ContractId: ContractId, TypeId: TypeId);
            var model = new ContractModel
            {
                lstContractAcceptance = items.Select(c => _contractModelFactory.PrepareTotalAcceptanceKL(c)).ToList(),
            };
            return PartialView(model);
        }
        public virtual IActionResult _ContractAcceptanceNoiBo(int ContractId)
        {
            var contract = _contractService.GetContractById(ContractId);
            var model = contract.ToModel<ContractModel>();
            _contractModelFactory.PrepareContractAcceptanceSub(model);
            return PartialView(model);
        }
        /// <summary>
        /// nghiem thu khoi luong
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="AcceptanceId"></param>       
        /// <param name="TypeId"></param>
        /// <param name="PaymentAccepId"></param>
        /// <returns></returns>
        public virtual IActionResult _ContractAcceptanceAdd(int ContractId = 0, int AcceptanceId = 0, int TypeId = (int)ContractAcceptancesType.KhoiLuong, int PaymentAccepId = 0)
        {
            var item = new ContractAcceptance();
            var model = new ContractAcceptanceModel
            {
                ContractId = ContractId,
                Id = AcceptanceId,
                TypeId = TypeId,
                PaymentAcceptanceId = PaymentAccepId
            };
            if (AcceptanceId > 0)
            {
                item = _contractService.getContractAcceptancebyId(AcceptanceId);
            }
            //if ((item != null) && (item.Id > 0))
            //{
            //    model = _contractModelFactory.PrepareContractAcceptanceModel(model, item, true);
            //}
            //else
            //{
            //    model = _contractModelFactory.PrepareContractAcceptanceModel(model: model, isDetail: true);
            //}           
            model = _contractModelFactory.PrepareContractAcceptanceKhoiLuong(model, item);
            return PartialView(model);
        }
        ///<summary>
        ///kiem tra list Task co cung 1 loai tien te
        /// </summary>
        public virtual IActionResult _CheckListCurrency(List<int> TaskIds)
        {
            var _listCurrencyId = _workTaskService.GetAllTasks().Where(c => TaskIds.Contains(c.Id)).Select(m => m.CurrencyId).Distinct().ToList();
            if (_listCurrencyId.Count > 1)
            {
                return JsonSuccessMessage("Ok");
            }
            return JsonErrorMessage("Error");
        }
        /// <summary>
        /// nghiem thu noi bo
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="AcceptanceId"></param>       
        /// <param name="TypeId"></param>
        /// <param name="PaymentAccepId"></param>
        /// <returns></returns>
        public virtual IActionResult _ContractAcceptanceNoiBoAdd(int ContractId = 0, int AcceptanceId = 0, int TypeId = (int)ContractAcceptancesType.NoiBo, int PaymentAccepId = 0)
        {
            var item = new ContractAcceptance();
            item = _contractService.getAllContractAcceptance(TypeId: (int)ContractAcceptancesType.NoiBo, PaymentAcceptanceId: PaymentAccepId, isFilter: true).FirstOrDefault();
            var model = new ContractAcceptanceModel
            {
                ContractId = ContractId,
                Id = AcceptanceId,
                TypeId = (int)ContractAcceptancesType.NoiBo,
                PaymentAcceptanceId = PaymentAccepId
            };
            if (AcceptanceId > 0)
            {
                item = _contractService.getContractAcceptancebyId(AcceptanceId);
            }
            if ((item != null) && (item.Id > 0))
            {
                model = _contractModelFactory.PrepareContractAcceptanceModel(model, item, true);
            }
            else
            {
                model = _contractModelFactory.PrepareContractAcceptanceModel(model: model, isDetail: true);
            }
            if (PaymentAccepId > 0)
            {
                model.listpaymentAcceptanceSub = _contractService.GetallPaymentAcceptanceSubs(PaymentAcceptanceId: PaymentAccepId).Select(c => c.ToModel<ContractPaymentAcceptanceSubModel>()).ToList();
            }
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractAcceptanceAdd(ContractAcceptanceModel model)
        {
            if (ModelState.IsValid)
            {
                var AcceptanceSubs = model.listAcceptanceSub;
                var noti = "admin.common.Added";
                int AcceptanceId = 0;
                var item = new ContractAcceptance();
                if (AcceptanceSubs.Count() > 0)
                {
                    model.TotalAmount = (AcceptanceSubs.Sum(c => c.TotalAmount.ToNumber())).ToString();
                }
                if (model.Id > 0)
                {
                    AcceptanceId = model.Id;
                    item = _contractService.getContractAcceptancebyId(model.Id);
                    _contractService.DeleteContractAcceptanceSubByAcceptanceId(AccepId: model.Id, TypeId: (int)ContractAcceptancesType.KhoiLuong);
                }
                _contractModelFactory.PrepareContractAcceptance(model, item);
                var noteLog = "";
                if (model.Id > 0)
                {
                    _contractService.UpdateContractAcceptance(item);
                    //Insert  Contract Log
                    var contractLog = new ContractLog() {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractAcceptanceModel>().toStringJson()
                    };
                    if (item.TypeId == (int)ContractAcceptancesType.BB)
                    {
                        noteLog = "Sửa thông tin nghiệm thu BB: ";
                    }
                    if (item.TypeId == (int)ContractAcceptancesType.KhoiLuong)
                    {
                        noteLog = "Sửa thông tin nghiệm thu khối lượng: ";
                    }
                    _contractLogService.InsertContractLog(contractLog, noteLog + item.Name);
                    //_contractService.DeleteContractAcceptanceTaskMappingbyAcceptanceId(model.Id);
                    _contractService.DeleteMultiContractFile(ContractId: model.ContractId, TypeId: (int)ContractFileType.TaskAcceptance, entityId: model.Id.ToString());
                    noti = "admin.common.Updated";
                }
                else
                {
                    _contractService.InsertContractAcceptance(item);
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractAcceptanceModel>().toStringJson()
                    };
                    if (item.TypeId == (int)ContractAcceptancesType.BB)
                    {
                        noteLog = "Thêm mới nghiệm thu BB: ";
                    }
                    if (item.TypeId == (int)ContractAcceptancesType.KhoiLuong)
                    {
                        noteLog = "Thêm mới nghiệm thu khối lượng: ";
                    }
                    _contractLogService.InsertContractLog(contractLog, noteLog + item.Name);
                    AcceptanceId = item.Id;
                }
                //Accepsub
                foreach (ContractAcceptanceSubModel subModel in model.listAcceptanceSub)
                {
                    var subItem = new ContractAcceptanceSub
                    {
                        AcceptanceId = item.Id,
                        TaskId = subModel.TaskId.GetValueOrDefault(0),
                        TotalAmount = subModel.TotalAmount.ToNumber(),
                        CreatorId = _workContext.CurrentCustomer.Id,
                        CreatedDate = DateTime.Now,
                    };
                    _contractModelFactory.PrepareContractAcceptanceKhoiLuongSub(subItem);
                    _contractService.InsertContractAcceptionSub(subItem);
                    //neu nghiem thu KL het gia tri hang muc thi thay doi trang thai cong viec
                    var  TaskAcceptanced = _contractService.GetallContractAcceptanceSubs(TaskId:(int)subModel.TaskId, TypeId: (int)ContractAcceptancesType.KhoiLuong).Sum(c=>c.TotalAmount);
                    var task  = _workTaskService.GetTaskById((int)subModel.TaskId);
                    var TaskTotalMonney = task.TotalMoney.GetValueOrDefault(0);
                    if (TaskAcceptanced >= TaskTotalMonney) {
                        task.StatusId =(int)TaskStatus.Acceptance;
                        _workTaskService.UpdateTask(task);
                    }
                    if (TaskAcceptanced <= TaskTotalMonney)
                    {
                        task.StatusId = (int)TaskStatus.Working;
                        _workTaskService.UpdateTask(task);
                    }
                }
                if (model.SelectedListFileId.Count > 0)
                {
                    foreach (int FileId in model.SelectedListFileId)
                    {
                        var contractFile = new ContractFile
                        {
                            ContractId = item.ContractId,
                            FileId = FileId,
                            TypeId = (int)ContractFileType.TaskAcceptance,
                            EntityId = item.Id.ToString(),
                        };
                        _contractService.InsertContractFile(contractFile);
                    }
                }
                //if (AcceptanceSubs.Count() > 0)
                //{
                //    foreach (ContractAcceptanceSubModel accSub in AcceptanceSubs)
                //    {
                //        accSub.AcceptanceId = item.Id;
                //        accSub.ContractId = item.ContractId;
                //        var accSubItem = new ContractAcceptanceSub();
                //        _contractModelFactory.PrepareContractAcceptanceSub(accSubItem, accSub);
                //        _contractService.InsertContractAcceptionSub(accSubItem);
                //    }
                //}
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<ContractAcceptanceModel>());
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
        }
        public virtual IActionResult _ContractAcceptanceTask(int ContractId, int TaskId = 0)
        {
            var searchmodel = new ContractAcceptanceSearchModel
            {
                TaskId = TaskId,
                ContractId = ContractId,
                TypeId = (int)ContractAcceptancesType.NoiBo,
            };
            var model = _contractModelFactory.PrepareContractAcceptanceListModel(searchmodel);
            return PartialView(model);
        }
        public virtual IActionResult DeleteContractAcceptance(int Id)
        {
            var item = _contractService.getContractAcceptancebyId(Id);
            if (item != null)
            {
                var TypeId = item.TypeId;
                var paymentAcceptanceSubs = _contractService.GetallPaymentAcceptanceSubs(AcceptanceId:Id);
                if (paymentAcceptanceSubs.Count >0)
                {
                    return JsonSuccessMessage("Đã tạo thanh toán không thể xoá !");
                }
                //doi trang thai hang muc cong viec sau khi xoa nghiem thu khoi luong
                if (item.TypeId == (int)ContractAcceptancesType.KhoiLuong)
                {
                    var tasks = _contractService.GetallContractAcceptanceSubs(AcceptanceId: Id, TypeId: (int)ContractAcceptancesType.KhoiLuong).Select(c=>c.task);                   
                    foreach (Task task in tasks)
                    {                       
                        task.StatusId = (int)TaskStatus.Working;
                        _workTaskService.UpdateTask(task);
                    }
                }
                _contractService.DeleteContractAcceptance(item);
                //Insert  Contract Log
                var contract = _contractService.GetContractById(item.ContractId);
                var contractlog = _contractModelFactory.PrepareContractLog(contract);
                _contractLogService.InsertContractLog(contractlog, "Xóa nghiệm thu khối lượng: " + item.Name);
                _contractService.DeleteContractAcceptanceSubByAcceptanceId(Id, TypeId);
            }
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), item.ToModel<ContractAcceptanceModel>());
        }

        #endregion
        #region ContractPaymentAcceptance
        public virtual IActionResult _ContractPaymentAcceptance(int ContractId)
        {
            var model = new ContractModel();
            var items = _contractService.getAllContractPaymentAcceptance(ContractId);
            model.lstContractPaymentAcceptance = items.Select(c => c.ToModel<ContractPaymentAcceptanceModel>()).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ContractPaymentAcceptanceAdd(int ContractId, int PaymenyAcceptanceId = 0)
        {
            var model = new ContractPaymentAcceptanceModel
            {
                ContractId = ContractId,
            };
            var item = new ContractPaymentAcceptance();
            if (PaymenyAcceptanceId > 0)
            {
                item = _contractService.getContractPaymentAcceptancebyId(PaymenyAcceptanceId);
            }
            model = _contractModelFactory.PrepareContractPaymentAcceptanceModel(model, item);
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractPaymentAcceptanceAdd(ContractPaymentAcceptanceModel model)
        {
            if (ModelState.IsValid)
            {
                var noti = "admin.common.Added";
                if (model.listPaymentAcceptanceSub.Count > 0)
                {
                    model.TotalAmount = model.listPaymentAcceptanceSub.Sum(c => (long)c.TotalMoney.ToNumber());
                    model.ReduceKeep = model.listPaymentAcceptanceSub.Sum(c => c.ReduceKeep.ToNumber());
                    model.ReduceOther = model.listPaymentAcceptanceSub.Sum(c => c.ReduceOther.ToNumber());
                    model.ReduceAdvance = model.listPaymentAcceptanceSub.Sum(c => c.ReduceAdvance.ToNumber());
                    model.Depreciation = model.listPaymentAcceptanceSub.Sum(c => c.Depreciation.ToNumber());
                }
                ContractPaymentAcceptance item = new ContractPaymentAcceptance();
                if (model.Id > 0)
                {
                    item = _contractService.getContractPaymentAcceptancebyId(model.Id);
                }
                _contractModelFactory.PrepareContractPaymentAcceptance(model, item);
                if (model.Id > 0)
                {
                    _contractService.DeletePaymentAcceptanceSubByPaymentAcceptanceId(model.Id);
                }
                if (model.Id > 0)
                {
                    _contractService.UpdateContractPaymentAcceptance(item);
                    //Insert  Contract Log
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractPaymentAcceptanceModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Sửa nghiệm thu thanh toán: " + item.Name);
                    noti = "admin.common.updated";
                }
                else
                {
                    _contractService.InsertContractPaymentAcceptance(item);
                    //Insert  Contract Log
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractPaymentAcceptanceModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Thêm mới nghiệm thu thanh toán: " + item.Name);
                }
                if (model.SelectedListFileId != null)
                {
                    foreach (int FileId in model.SelectedListFileId)
                    {
                        var contractFile = new ContractFile
                        {
                            ContractId = item.ContractId,
                            FileId = FileId,
                            TypeId = (int)ContractFileType.PaymentAcceptance,
                            EntityId = item.Id.ToString(),
                        };
                        _contractService.InsertContractFile(contractFile);
                    }
                }

                //foreach (int acId in model.SelectedContractAcceptanceIds)
                //{
                //    _contractService.UpdateContractAcceptancebypayment(acId, item.Id);
                //}
                foreach (ContractPaymentAcceptanceSubModel ps in model.listPaymentAcceptanceSub)
                {
                    var Subitem = new ContractPaymentAcceptanceSub
                    {
                        ContractId = model.ContractId,
                        PaymentAcceptanceId = item.Id,
                    };
                    _contractModelFactory.PrepareContractPaymentAcceptanceSub(Subitem, ps);
                    _contractService.InsertContractPaymentAcceptionSub(Subitem);
                }
                UpdateContractUnfinishbyPayment(item.Id);
                return JsonSuccessMessage(_localizationService.GetResource(noti));
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
        }
        [HttpPost]
        public virtual IActionResult _GetTotalAmountPaymentAcceptane(ContractPaymentRequestModel model)
        {
            var items = _contractService.GetPaymentAcceptancebyListAcceptanceIds(model.SelectedContractPaymentAcceptence.ToList());
            return JsonSuccessMessage("ok", items.Sum(c => c.TotalAmount - c.ReduceAdvance - c.ReduceKeep - c.ReduceOther));
        }
        public virtual IActionResult DeletePaymentAcceptance(int Id)
        {
            var item = _contractService.getContractPaymentAcceptancebyId(Id);
            if (item != null)
            {
                //_contractService.DeleteContractAcceptanceTaskMappingbyAcceptanceId(Id);
                _contractService.DeleteContractPaymentAcceptance(item);
                //Insert  Contract Log
                var contractLog = new ContractLog()
                {
                    ContractId = item.Id,
                    CreatedDate = DateTime.Now,
                    CreatorId = _workContext.CurrentCustomer.Id,
                    ContractData = item.ToModel<ContractPaymentAcceptanceModel>().toStringJson()
                };
                _contractLogService.InsertContractLog(contractLog, "Xóa nghiệm thu thanh toán: " + item.Name);
                _contractService.DeletePaymentAcceptanceSubByPaymentAcceptanceId(item.Id);
            }
            UpdateContractUnfinishbyPayment(item.Id);
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"));
        }

        #endregion
        #region ContractPaymentAcceptanceSub      
        public virtual IActionResult _ContractPaymentAcceptanceListSubAdd(int AcepptanceId)
        {
            //var acepptanceMaps = _contractService.GetAllContractAcceptanceTaskMapping(AcepptanceId: AcepptanceId);
            //get all ContractAcceptanceSub khoi luong
            var acepptances = _contractService.GetallContractAcceptanceSubs(AcceptanceId: AcepptanceId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            var model = acepptances.Select(c => _contractModelFactory.prepareContractPaymentAcceptancebyTaskMap(c)).ToList();
            return PartialView(model);
        }
        public virtual IActionResult _ContractPaymentAcceptanceSub(int PaymentAcceptanceId)
        {
            var model = _contractService.GetallPaymentAcceptanceSubs(PaymentAcceptanceId).Select(c => c.ToModel<ContractPaymentAcceptanceSubModel>()).ToList();
            return PartialView(model);
        }

        #endregion
        #region ContractAcceptanceBB
        public virtual IActionResult _DeleteAcceptanceBB(int Id)
        {
            var item = _contractService.getContractAcceptanceBBbyId(Id);
            if (item != null)
            {
                _contractService.DeleteContractAcceptanceBB(item);
                return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), item.ToModel<ContractAcceptanceBBModel>());
            }
            return JsonErrorMessage();
        }
        public virtual IActionResult _ContractAcceptanceBBAdd(int Id = 0, int TaskId = 0, int ContractId = 0)
        {
            var item = new ContractAcceptanceBB();
            if (Id > 0)
            {
                item = _contractService.getContractAcceptanceBBbyId(Id);
            }
            var model = new ContractAcceptanceBBModel()
            {
                TaskId = TaskId,
                ContractId = ContractId
            };

            model = _contractModelFactory.PrepareContractAcceptanceBBModel(model, item);
            return PartialView(model);

        }
        [HttpPost]
        public virtual IActionResult _ContractAcceptanceBBAdd(ContractAcceptanceBBModel model)
        {
            if (ModelState.IsValid)
            {
                var noti = "admin.common.Added";
                var item = new ContractAcceptanceBB();
                if (model.Id > 0)
                {
                    item = _contractService.getContractAcceptanceBBbyId(model.Id);
                }
                _contractModelFactory.PrepareContractAcceptanceBB(model, item);
                if (model.Id > 0)
                {
                    _contractService.UpdateContractAcceptanceBB(item);
                    noti = "admin.common.Updated";
                }
                else
                {
                    _contractService.InsertContractAcceptanceBB(item);
                }
                if (model.SelectedListFileId != null)
                {
                    foreach (int FileId in model.SelectedListFileId)
                    {
                        var contractFile = new ContractFile
                        {
                            ContractId = item.ContractId,
                            FileId = FileId,
                            TypeId = (int)ContractFileType.BBAcceaptance,
                            EntityId = item.Id.ToString(),
                        };
                        _contractService.InsertContractFile(contractFile);
                    }
                }
                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<ContractAcceptanceBBModel>());
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
        }
        public virtual IActionResult _ContractAcceptanceBB()
        {
            var model = new ContractAcceptanceBBModel();
            return PartialView(model);
        }
        #endregion
        #region ContractAcceptanceSub
        public virtual IActionResult _ContractAcceptanceSub(int TaskId, int ContractId = 0, int PaymentAcceptanceSubId = 0, int ContractAcceptanceId = 0)
        {

            var model = new ContractAcceptanceModel();
            var PaymentAcceptanceSub = _contractService.GetContractPaymentAcceptanceSubById(PaymentAcceptanceSubId).ToModel<ContractPaymentAcceptanceSubModel>();
            var tasks = _workTaskService.getTasksByConTractId(ContractId: ContractId, ParentId: TaskId);
            if (tasks.Count == 0)
            {
                var task = _workTaskService.GetTaskById(TaskId);
                model.listAcceptanceSub.Insert(0, _contractModelFactory.PrepareAcceptanceSubModelbyTask(task, PaymentAcceptanceSub, ContractAcceptanceId));
            }
            else
            {
                model.listAcceptanceSub = tasks.Select(c => _contractModelFactory.PrepareAcceptanceSubModelbyTask(c, PaymentAcceptanceSub, ContractAcceptanceId)).ToList();
            }
            return PartialView(model);
        }
        public virtual IActionResult _ContractAcceptanceSubAdd(int contractAcceptanceId = 0, int contractId = 0, int taskId = 0)
        {
            var model = new ContractAcceptanceSubModel()
            {
                AcceptanceId = contractAcceptanceId,
                ContractId = contractId,
                TaskId = taskId
            };
            model = _contractModelFactory.PrepareContractAcceptanceSubModel(model, null);
            return PartialView(model);
        }
        public virtual IActionResult _ContractAcceptanceKhoiLuongSubAdd(int contractAcceptanceId = 0, int contractId = 0, int taskId = 0)
        {
            var model = new ContractAcceptanceSubModel()
            {
                AcceptanceId = contractAcceptanceId,
                ContractId = contractId,
                TaskId = taskId
            };
            model = _contractModelFactory.PrepareContractAcceptanceKhoiLuongSubModel(model, null);
            return PartialView(model);
        }
        public virtual IActionResult DeleteContractAcceptanceSub(int Id)
        {
            var item = _contractService.GetContractAcceptanceSubById(Id);
            _contractService.DeleteContractAcceptanceSub(item);
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), item.ToModel<ContractAcceptanceSubModel>());
        }
        #endregion
       
        #region ContractSettlement
        public virtual IActionResult DeleteContractSettlement(int Id)
        {
            var item = _contractService.getContractSettlementbyId(Id);
            if (item != null)
            {
                var lst = _contractService.GetListTaskIdBySettlementId(Id);
                foreach (var t in lst)
                {
                    var task = _workTaskService.GetTaskById(t);
                    task.StatusId = (int)TaskStatus.Working;
                    _workTaskService.UpdateTask(task);
                    // update status ChilTask
                    var chilTask = _workTaskService.getTasksByConTractId(ParentId: t);
                    foreach (var c in chilTask)
                    {
                        c.StatusId = task.StatusId;
                        _workTaskService.UpdateTask(c);
                    }
                }
                _contractService.DeleteContractSettlement(item);
                //Insert  Contract Log
                var contractLog = new ContractLog()
                {
                    ContractId = item.Id,
                    CreatedDate = DateTime.Now,
                    CreatorId = _workContext.CurrentCustomer.Id,
                    ContractData = item.ToModel<ContractSettlementModel>().toStringJson()
                };
                _contractLogService.InsertContractLog(contractLog, "Xóa quyết toán: " + item.Name);
            }
            var sub = _contractService.GetSubBySettlementId(Id);
            if (sub != null)
            {
                foreach (var s in sub)
                {
                    _contractService.DeleteContractSettlementSub(s);
                }
            }
            var contract = _contractService.GetContractById(item.ContractId);
            contract.StatusId = (int)ContractStatus.Building;
            _contractService.UpdateContract(contract);
            return JsonSuccessMessage(_localizationService.GetResource("admin.common.Deleted"), item.ToModel<ContractSettlementModel>());
        }
        public virtual IActionResult _ContractSettlement(int ContractId)
        {
            var items = _contractService.getAllContractSettlement(ContractId);
            var model = new ContractModel
            {
                lsContractSettlement = items.Select(c => _contractModelFactory.prepareTotalAmountContractSettle(c)).ToList()
            };
            return PartialView(model);
        }
        public virtual IActionResult _ContractSettlementAdd(int ContractId = 0, int Id = 0)
        {
            var item = new ContractSettlement();
            var model = new ContractSettlementModel
            {
                ContractId = ContractId,
                Id = Id
            };

            if (Id > 0)
            {
                item = _contractService.getContractSettlementbyId(Id);
                model = item.ToModel<ContractSettlementModel>();
                var lstsub = _contractService.GetSubBySettlementId(Id);
                model.lstSubModel = lstsub.Select(c => c.ToModel<ContractSettlementSubModel>()).ToList();
                model.SelectedListTaskId = lstsub.Select(c => c.TaskId).ToList();
                foreach (ContractSettlementSubModel css in model.lstSubModel)
                {
                    css.TaskName = _workTaskService.GetTaskById(css.TaskId).Name;
                    css.TaskTotalMoney = _workTaskService.GetTaskById(css.TaskId).TotalMoney.ToVNStringNumber();
                    css.UnitName = _workTaskService.GetTaskById(css.TaskId).unitInfo != null ? _workTaskService.GetTaskById(css.TaskId).unitInfo.Name : "";
                }
                var listFile = _contractService.GetallContractFiles(ContractId: ContractId, TypeId: (int)ContractFileType.Settlement, EntityId: Id);
                model.WorkFileIds = String.Join(',', listFile.Select(c => c.FileId).ToList());
            }

            // check Task đã quyết toán
            var tasks = _workTaskService.getTasksByConTractId(ContractId: ContractId, ParentId: 0).Where(c=>c.TaskProcuringAgencyId == null);
            var listTaskId = _contractService.GetTaskByContractIdInSettlementSub(ContractId);
            tasks = tasks.Where(c => !listTaskId.Contains(c.Id)).ToList();
            if (Id > 0)
            {
                var listTaskIds = _contractService.GetSubBySettlementId(Id).Select(c => c.TaskId).ToList();
                var listTasks = _workTaskService.GetTaskbyListTaskIds(listTaskIds);
                if (tasks != null)
                {
                    tasks = tasks.Union(listTasks).ToList();
                }
            }
            model.AvailableTaskList = tasks.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.TaskId
            }).ToList();
            return PartialView(model);
        }
        [HttpPost]
        public virtual IActionResult _ContractSettlementAdd(ContractSettlementModel model)
        {
            if (ModelState.IsValid && model.lstSubModel.Count > 0)
            {
                var noti = "admin.common.Added";
                var item = new ContractSettlement();

                if (model.Id > 0)
                {
                    item = _contractService.getContractSettlementbyId(model.Id);
                    _contractService.DeleteContractSettlementSubBySettlementId(model.Id);
                }
                _contractModelFactory.PrepareContractSettlement(model, item);
                if (model.Id > 0)
                {
                    _contractService.UpdateContractSettlement(item);
                    //Insert  Contract Log
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractSettlementModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Sửa quyết toán: " + item.Name);
                    noti = "admin.common.Updated";
                }
                else
                {
                    _contractService.InsertContractSettlement(item);
                    //Insert  Contract Log
                    var contractLog = new ContractLog()
                    {
                        ContractId = item.Id,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id,
                        ContractData = item.ToModel<ContractSettlementModel>().toStringJson()
                    };
                    _contractLogService.InsertContractLog(contractLog, "Thêm mới quyết toán: " + item.Name);
                }
                foreach (ContractSettlementSubModel submodel in model.lstSubModel)
                {
                    var subitem = new ContractSettlementSub
                    {
                        TotalAmount = submodel.TotalAmount.ToNumber(),
                        TaskId = submodel.TaskId,
                        ContractSettlementId = item.Id,
                        ContractId = item.ContractId,
                        CreatedDate = DateTime.Now,
                        CreatorId = _workContext.CurrentCustomer.Id
                    };
                    // update status task
                    var task = _workTaskService.GetTaskById(submodel.TaskId);
                    task.StatusId = (int)TaskStatus.Settlemented;
                    _workTaskService.UpdateTask(task);
                    // update status ChilTask
                    var chilTask = _workTaskService.getTasksByConTractId(ParentId: submodel.TaskId);
                    foreach (var t in chilTask)
                    {
                        t.StatusId = (int)TaskStatus.Settlemented;
                        _workTaskService.UpdateTask(t);
                    }
                    _contractService.InsertContractSettlementSub(subitem);
                }
                if (model.SelectedListFileId != null)
                {
                    foreach (int FileId in model.SelectedListFileId)
                    {
                        var contractFile = new ContractFile
                        {
                            ContractId = item.ContractId,
                            FileId = FileId,
                            TypeId = (int)ContractFileType.Settlement,
                            EntityId = item.Id.ToString(),
                        };
                        _contractService.InsertContractFile(contractFile);
                    }
                }
                var lstTask = _workTaskService.getTasksByConTractId(ContractId: item.ContractId, ParentId: 0)
                    .Where(c => c.StatusId != (int)TaskStatus.Settlemented).Count();
                if (lstTask == 0)
                {
                    var contract = _contractService.GetContractById(item.ContractId);
                    contract.StatusId = (int)ContractStatus.Settlemented;
                    _contractService.UpdateContract(contract);
                }

                return JsonSuccessMessage(_localizationService.GetResource(noti), item.ToModel<ContractSettlementModel>());
            }
            var list = ModelState.Values.Where(c => c.Errors.Count > 0).ToList();
            return JsonErrorMessage("Error", list);
        }
        #endregion
        #region ContractSettlementSub
        public virtual IActionResult _ContractSettlementSubAdd(int taskId)
        {
            var query = _taskService.GetTaskById(taskId);
            var model = new ContractSettlementSubModel()
            {
                TaskId = taskId,
                TaskName = query.Name,
                TaskTotalMoney = query.TotalMoney.ToVNStringNumber(),
                UnitName = _taskService.GetTaskById(taskId).unitInfo != null ? _taskService.GetTaskById(taskId).unitInfo.Name : ""
            };

            return PartialView(model);
        }
        #endregion
        #region UpdateContractunfinish
        public void UpdateContractUnfinish(int Id)
        {
            var notiId = _dataProvider.GetParameter();
            notiId.ParameterName = "Id";
            notiId.Value = Id;
            notiId.DbType = DbType.Int32;
            _dbContext.ExecuteSqlCommand("EXEC [dbo].[SP_ContractUnfinishContractId] @Id",
               false, 600, notiId);
        }
        public void UpdateContractUnfinishbyTypeId(int ContractId, int TypeId)
        {
            var contractId = _dataProvider.GetParameter();
            var typeId = _dataProvider.GetParameter();
            contractId.ParameterName = "contractId";
            typeId.ParameterName = "typeId";
            contractId.Value = ContractId;
            contractId.DbType = DbType.Int32;
            typeId.Value = TypeId;
            typeId.DbType = DbType.Int32;
            _dbContext.ExecuteSqlCommand("EXEC [dbo].[SP_ContractUnfinishByContractIdTypeId] @contractId,@typeId",
               false, 600, contractId, typeId);
        }
        public void UpdateContractUnfinishbyPayment(int PaymentAcceptanceId)
        {
            var paymentId = _dataProvider.GetParameter();
            paymentId.ParameterName = "PaymentAcceptanceId";
            paymentId.Value = PaymentAcceptanceId;
            paymentId.DbType = DbType.Int32;
            _dbContext.ExecuteSqlCommand("EXEC [dbo].[SP_ContractUnfinishByPaymentAcceptanceId] @PaymentAcceptanceId",
               false, 600, paymentId);
        }
        #endregion
    }
}
