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
using GS.Core.Configuration;

namespace GS.Web.Factories
{
    public class TaskModelFactory : ITaskModelFactory
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
        private readonly IContractPaymentService _contractPaymentService;
        private readonly IContractLogService _contractLogService;
        private readonly IUnitService _unitService;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IContractTypeService _contractTypeService;
        private readonly GSConfig _config;
        #endregion
        #region Ctor

        public TaskModelFactory(ICurrencyService currencyService,
            ITaskGroupService taskGroupService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IWorkContext workContext,
            IWorkTaskService workTaskService,
            ICustomerService customerService,
            IContractService contractService,
            IContractPaymentService contractPaymentService,
            IContractLogService contractLogService,
            IUnitService unitService,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            IPriceFormatter priceFormatter,
        IGenericAttributeService genericAttributeService,
         IContractTypeService contractTypeService,
          GSConfig config)
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
            this._contractPaymentService = contractPaymentService;
            this._contractService = contractService;
            this._contractLogService = contractLogService;
            this._unitService = unitService;
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
            this._genericAttributeService = genericAttributeService;
            this._priceFormatter = priceFormatter;
            this._contractTypeService = contractTypeService ;
            this._config = config;
        }

        #endregion
        #region Customer
        string GetCustomerAvatarUrl(Customer customer)
        {
            return _pictureService.GetPictureUrl(
        _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.AvatarPictureIdAttribute),
        _mediaSettings.AvatarPictureSize, defaultPictureType: PictureType.Avatar);
        }
        #endregion
        #region Task Group
        public TaskGroupSearchModel PrepareTaskGroupSearchModel(TaskGroupSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public TaskGroupListModel PrepareTaskGroupListModel(TaskGroupSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _taskGroupService.GetAllTaskGroupsByName(searchModel.Name, searchModel.ParentId);

            //prepare list model
            var model = new TaskGroupListModel
            {
                //fill in model values from the entity
                Data = items.Select(store => store.ToModel<TaskGroupModel>()),
                Total = items.Count
            };

            return model;
        }

        public TaskGroupModel PrepareTaskGroupModel(TaskGroupModel model, TaskGroup item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<TaskGroupModel>();
            }
            var items = _taskGroupService.GetAllTaskGroups();
            string treetam = "";
            model.AvaibleTaskGroupList = items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = treetam.PadLeft(c.TreeLevel, '-') + c.Name,
                Selected = c.Id == model.ParentId
            }).ToList();
            model.AvaibleTaskGroupList.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "--Chọn tỷ lệ khoán cha--",

            });
            return model;
        }

        #endregion
        #region task
        public TaskSearchModel PrepareTaskSearchModel(TaskSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public TaskListModel PrepareTaskListModel(TaskSearchModel searchModel)
        {
            var items = _workTaskService.getTasksByConTractId(searchModel.ContractId, searchModel.ParentId).Select(c =>
            {
                var m = PrepareTaskModelList(c);
                m.taskStatusText = _localizationService.GetLocalizedEnum(c.taskStatus);
                return m;
            }).ToList();
            var model = new TaskListModel
            {
                Data = items,
                Total = items.Count
            };
            return model;
        }

        public TaskModel PrepareTaskModel(TaskModel model, Task item, bool excludeProperties = false)
        {
            Task _parentask = null;
            if (item.Id > 0)
            {
                model = item.ToModel<TaskModel>();
                model.taskStatusText = _localizationService.GetLocalizedEnum(item.taskStatus);
                var _taskAcceptanceKL = _contractService.GetallContractAcceptanceSubs(TaskId: item.Id, TypeId: (int)ContractAcceptancesType.KhoiLuong);
                if (_taskAcceptanceKL.Count > 0)
                {
                    model.isAcceptanceKL = true;
                }
            }
            else
            {
                //neu tao moi cong viec con, lay time bat dau va ket thuc gan cho cong viec con
                if (model.ParentId > 0)
                {
                    _parentask = _workTaskService.GetTaskById(model.ParentId);
                    model.StartDate = _parentask.StartDate.HasValue ? _parentask.StartDate.Value : DateTime.Now;
                    model.EndDate = _parentask.EndDate;
                }
            }
            if (model.CurrencyId == 0)
            {
                //kiem tra xem task co phai la task con ko ?
                if (model.ParentId > 0)
                {
                    //neu la task con thi lay mac dinh tien te theo tien te cua task cha
                    if (_parentask == null)
                        _parentask = _workTaskService.GetTaskById(model.ParentId);
                    model.CurrencyId = _parentask.CurrencyId.GetValueOrDefault(0);
                    model.CurrencyName = _parentask.currency.Name;
                }
                if (model.CurrencyId == 0)
                {
                    //lay thong tin tien te theo hop dong
                    var contract = _contractService.GetContractById(model.ContractId).ToModel<ContractModel>();
                    if (contract != null)
                    {
                        model.CurrencyName = contract.CurrencyName;
                        model.CurrencyId = contract.CurrencyId;
                    }
                }

            }
            if (model.Id == 0)
            {
                // lay ngay ky hop dong cho ngay bat dau cong viec
                var _contract = _contractService.GetContractById(model.ContractId);
                model.StartDate = (DateTime)_contract.SignedDate;
                model.EndDate = (DateTime)_contract.EndDate;
                model.ContractEndDate = (DateTime)_contract.EndDate;
                model.ContractStarDate = (DateTime)_contract.StartDate;
            }
            model.lsTaskLevelId = ((ConTractTaskLeverId)model.TaskLevelId).ToSelectList();
            model.AvailableTaskGroup = _taskGroupService.GetAllTaskGroups().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = model.TaskGroupId == c.Id
            }).ToList();
            model.AvailableTaskGroup.Insert(0, (new SelectListItem
            {
                Text = "Chọn tỷ lệ khoán",
                Value = "0",
            }));
            string treeTam = "";
            //phai bo task dang sua ra khoi danh sach cha, tranh truong hop parentId=Model.Id
            model.AvailableTask = _workTaskService.getTasksByConTractId(ContractId: model.ContractId, isAll: true).Where(c => c.Id != model.Id).OrderBy(c => c.TreeNode).Select(c => new SelectListItem
            {
                Text = treeTam.PadLeft(c.TreeLevel - 1, '-') + c.Name,
                Value = c.Id.ToString(),
                Selected = model.ParentId == c.Id
            }).ToList();
            model.AvailableTask.Insert(0, (new SelectListItem
            {
                Text = "",
                Value = "0",
            }));
            model.AvailableUnit = _unitService.GetAllUnits().Select(c => new SelectListItem
            {
                Text = treeTam.PadLeft((int)c.TreeLevel, '-') + c.Name,
                Value = c.Id.ToString(),
                Selected = model.UnitId == c.Id
            }).ToList();
            model.AvailableUnit.Insert(0, (new SelectListItem
            {
                Text = "Chọn đơn vị thực hiện",
                Value = "0",
            }));
            var ContractTypeMaps = _contractTypeService.GetContractTypeMapping(ContractId:model.ContractId);            
           // var contracttypes = _contractTypeService.GetAllContractTypes();
            model.AvailebleContractType = ContractTypeMaps.Select(c => new SelectListItem
            {
                Value = c.contractType.Id.ToString(),
                Text = c.contractType.Name,
                Selected = model.ContractTypeId == c.contractType.Id
            }).ToList();
            model.AvailebleContractType.Insert(0, new SelectListItem
            {
                Text = "---chọn loại hợp đồng---",
                Value = "0"
            });
            model.AvailableCurrency = _currencyService.GetAllCurrencies().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.CurrencyId
            }).ToList();
            if (item.Id>0)
            {
                model.TaskGroupValue = Math.Round(item.TaskGroupValue.GetValueOrDefault(0) * 100, 2);
                model.PercentNum = Math.Round(item.PercentNum.GetValueOrDefault(0) * 100, 2);
                model.TaxPercent = Math.Round(item.TaxPercent.GetValueOrDefault(0) * 100, 2);
                model.TaskGroupCost = Math.Round(item.TaskGroupCost.GetValueOrDefault(0) * 100, 2);
                model.TaskGroupSalary = Math.Round(item.TaskGroupSalary.GetValueOrDefault(0) * 100, 2);
            }            
            if (excludeProperties)
            {
                if (item.Id > 0)
                {
                    var advanceQuantity = _contractService.getAllContractAcceptance(ContractId: item.ContractId, TaskId: item.Id, TypeId: (int)ContractAcceptancesType.TamUng).Select(c => c.ToModel<ContractAcceptanceModel>()).ToList();
                    model.listAdvanceQuantity = new ContractAcceptanceListModel
                    {
                        Data = advanceQuantity,
                        Total = advanceQuantity.Count,
                    };
                    //tinh gia tri tam ung san lương
                    model.AdvanceQuantityValue = _priceFormatter.FormatPrice(advanceQuantity.Sum(c => c.TotalAmount.ToNumber()), false, item.currency);
                    // Hop dong BB cua cong viec
                    var bbs = item.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract.ToModel<ContractModel>()).ToList();
                    model.listBBContract = new ContractListModel
                    {
                        Data = bbs,
                        Total = bbs.Count,
                    };
                    // Hop dong phu luc cua cong viec
                    var Appendixs = item.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.Appendix && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract.ToModel<ContractModel>()).ToList();
                    model.listAppendixIdContract = new ContractListModel
                    {
                        Data = Appendixs,
                        Total = Appendixs.Count
                    };
                    var ContractAcceptances = _contractService.getAllContractAcceptance(ContractId: item.ContractId, TaskId: item.Id, TypeId: (int)ContractAcceptancesType.NoiBo).Select(c => c.ToModel<ContractAcceptanceModel>()).ToList();
                    model.listContractAcceptanceTask = new ContractAcceptanceListModel
                    {
                        Data = ContractAcceptances,
                        Total = ContractAcceptances.Count
                    };
                    PrepareDeadline(model, item);
                }
            }
            PrepareTaskResource(model);
            return model;
        }
        public TaskModel PrepareTaskModelList(Task item)
        {
            var model = item.ToModel<TaskModel>();
            if (item.contractType != null)
            {
                model.ContractTypeName = item.contractType.Name;
            }           
            model.TaskGroupValue = Math.Round(item.TaskGroupValue.GetValueOrDefault(0) * 100, 2);
            model.PercentNum = Math.Round(item.PercentNum.GetValueOrDefault(0) * 100, 2);
            decimal ratio = _config.TaskGroupValueDefault;
            if (item.TaskGroupValue > 0)
                ratio = model.TaskGroupValue;
            model.TotalLimitAdvance = _priceFormatter.FormatPrice((Int64)(item.TotalMoney.GetValueOrDefault(0) * ratio/100), false, item.currency);
            var advanceQuantity = _contractService.getAllContractAcceptance(ContractId: item.ContractId, TaskId: item.Id, TypeId: (int)ContractAcceptancesType.TamUng).Select(c => c.ToModel<ContractAcceptanceModel>()).ToList();
            model.listAdvanceQuantity = new ContractAcceptanceListModel
            {
                Data = advanceQuantity,
                Total = advanceQuantity.Count,
            };
            //tinh gia tri tam ung san lương
            model.AdvanceQuantityValue = _priceFormatter.FormatPrice((Int64)advanceQuantity.Sum(c => c.TotalAmount.ToNumber()), false, item.currency);
            // Hop dong BB cua cong viec
            var bbs = item.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract.ToModel<ContractModel>()).ToList();           
            model.listBBContract = new ContractListModel
            {
                Data = bbs,
                Total = bbs.Count,
            };
            // Hop dong phu luc cua cong viec
            var Appendixs = item.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.Appendix && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract.ToModel<ContractModel>()).ToList();
            model.listAppendixIdContract = new ContractListModel
            {
                Data = Appendixs,
                Total = Appendixs.Count
            };
            var ContractAcceptances = _contractService.getAllContractAcceptance(ContractId: item.ContractId, TaskId: item.Id, TypeId: (int)ContractAcceptancesType.NoiBo).Select(c => c.ToModel<ContractAcceptanceModel>()).ToList();
            model.listContractAcceptanceTask = new ContractAcceptanceListModel
            {
                Data = ContractAcceptances,
                Total = ContractAcceptances.Count
            };
            //tinh gia tri da tam ung 
            var payments = _contractPaymentService.GetContractPayments(taskId:item.Id,IsReceipts:2);
            model.TotalAdvanced = _priceFormatter.FormatPrice((Int64)payments.Sum(c=>c.Amount), false, item.currency);

            //if (c.TaskGroupValue != 0)
            //{
            //    m.TotalAdvancedAcceptanceNoiBo = (c.TotalMoney - (_contractPaymentService.GetAllContractsPaymentAmountByTaskId(c.Id) * 100 / ((decimal)c.TaskGroupValue * 100))).ToVNStringNumber() + " " + m.CurrencyCode;
            //}
            //else
            //{
            //    m.TotalAdvancedAcceptanceNoiBo = (c.TotalMoney - _contractPaymentService.GetAllContractsPaymentAmountByTaskId(c.Id)).ToVNStringNumber() + " " + m.CurrencyCode;
            //}
            if (model.listBBContract.Total > 0)
            {
                var contractAcceptanceBB = _contractService.getAllContractAcceptanceBB(TaskId: model.Id).FirstOrDefault();
                if (contractAcceptanceBB!=null)
                {
                    model.contractAcceptanceBB = contractAcceptanceBB.ToModel<ContractAcceptanceBBModel>();
                }
                var paymentBBs = _contractPaymentService.GetAllContractsPayment(TaskId:item.Id);
                model.listPaymentBB = paymentBBs.Select(c => c.ToModel<ContractPaymentModel>()).ToList();
                model.TotalAmountBBText = _priceFormatter.FormatPrice((Int64)paymentBBs.Sum(c => c.Amount), false, item.currency);
            }
            //tinh gia tri da nghiem thu khoi luong
            var acceptancesubs = _contractService.GetallContractAcceptanceSubs(TaskId: model.Id, TypeId:(int)ContractAcceptancesType.KhoiLuong);
            model.TotalAmountAcceptanced = acceptancesubs.Sum(c => c.TotalAmount).ToVNStringNumber();
            //tinh gia tri da nghiem thu thanh toan
            var paymentacceptancesubs = _contractService.GetallPaymentAcceptanceSubs(TaskId: model.Id);
            model.TotalAmountPaymentAcceptanced = paymentacceptancesubs.Sum(c => c.TotalAmount.GetValueOrDefault(0)).ToVNStringNumber();
            //tinh gia tri da quyet toan
            var ContractSettlementes = _contractService.GetAllContractSettlementSub(taskId: model.Id);
            model.TaskSettlemented = ContractSettlementes.Sum(c => c.TotalAmount).ToVNStringNumber();
            PrepareDeadline(model, item);
            PrepareUnfinished(item,model);
            return model;
        }
        public void PrepareDeadline(TaskModel model, Task item)
        {
            if (item.EndDate == null)
            {
                model = null;
            }
            else
            {
                var date = item.EndDate - DateTime.Now;
                TimeSpan timeSpan = date.Value;
                double time = timeSpan.Days;
                if(time < 0)
                {
                    model.classStatus = "badge-danger";
                    model.nameStatus = "Quá hạn";
                    model.countDown = "";
                }
                if (time >= 0 && time <= 7)
                {
                    model.classStatus = "badge-warning";
                    model.nameStatus = "Sắp đến hạn";
                    if (time == 0)
                    {
                        model.countDown = "(1)";
                    }
                    else
                    {
                        model.countDown = "(" + (time + 1).ToString() + ")";
                    }
                }
                if (time > 7)
                {
                    model.classStatus = "badge-success";
                    model.nameStatus = "Đang thực hiện";
                    model.countDown = "";
                }
            }
        }
        public void PrepareTask(Task item, TaskModel model)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.ContractId = model.ContractId;
            }
            item.Name = model.Name;
            item.Description = model.Description;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.MeasureMass = model.MeasureMass;
            item.MeasurePrice = model.MeasurePrice;
            item.TotalMoney = model.TotalMoney.ToNumber();
            item.TaskGroupValue = model.TaskGroupValue / 100;
            item.PercentNum = model.PercentNum / 100;
            item.ContractTypeId = model.ContractTypeId;
            item.TaxPercent = model.TaxPercent / 100;
            item.TaskGroupCost = model.TaskGroupCost / 100;
            item.TaskGroupSalary = model.TaskGroupSalary / 100;
            if (model.ResponsibleId > 0)
            {
                item.ResponsibleId = model.ResponsibleId;
            }
            if (model.UnitId > 0)
            {
                item.UnitId = model.UnitId;
            }
            else
            {
                item.UnitId = null;
            }
            // kiem tra hang muc cha va chon don vị theo hang muc cha
            if (model.ParentId > 0)
            {
                item.ParentId = model.ParentId;
                var taskParent = _workTaskService.GetTaskById(model.ParentId);
                item.TreeLevel = taskParent.TreeLevel+1;
                if (model.UnitId == 0)
                {                  
                    if (taskParent != null && taskParent.UnitId > 0)
                    {
                        item.UnitId = taskParent.UnitId;
                    }
                    if (taskParent != null && taskParent.ContractTypeId > 0)
                    {
                        item.ContractTypeId = taskParent.ContractTypeId;
                    }
                }
            }
            else
            {
                item.ParentId = null;
                item.TreeLevel = 1;
            }
            if (model.ContractTypeId>0) {               
                item.ContractTypeId = model.ContractTypeId;
            }
            else
            {
                item.ContractTypeId = null;
            }
            item.TaskLevelId = model.TaskLevelId;
            if (model.CurrencyId > 0)
            {
                item.CurrencyId = model.CurrencyId;
            }
            if (model.TaskGroupId > 0)
            {
                item.TaskGroupId = model.TaskGroupId;
            }
            else
            {
                item.TaskGroupId = null;
            }
            if (model.FinishedDate != null)
            {
                item.EndDate = model.EndDate;
            }
        }
        /// <summary>
        /// tinh gia tri dang do
        /// </summary>
        /// <param name="model"></param>
        public void PrepareUnfinished(Task item,TaskModel model)
        {            
            //neu chua den ngay thuc hien thi bang gia tri cua cong viec
            //dang do chua thuc hien
            if (item.StartDate > DateTime.Now || item.StartDate == null)
            {
                model.Unfinished = item.TotalMoney.GetValueOrDefault(0);
                model.UnfinishedText = _priceFormatter.FormatPrice(model.Unfinished, true, item.currency);
            }
            //neu da den ngay thi tinh theo tien do - gia ti nhiem thu
            else
            {                
                ///so ngay cua cong viec
                decimal intervalDaysTask = ((DateTime)model.EndDate).Subtract(model.StartDate).Days + 1;
                decimal interval = DateTime.Now.Subtract(model.StartDate).Days + 1;
                ///so ngay den hien tai            
                //var intervalDaysPerformTask = interval.Days+1;
                 var spandate = Math.Round((interval/intervalDaysTask),3, MidpointRounding.AwayFromZero);
                if (spandate > 1)
                {
                    spandate = 1;
                }
                //gia tri da NTTT cua cong viec
                var Accepted = _contractService.GetallPaymentAcceptanceSubs(TaskId: model.Id);
                var percent = _workTaskService.GetTaskById(model.Id).TaxPercent;
                if (percent == null) percent = 0;
                var TotalAmountAccepted = Accepted.Sum(c => c.TotalMoney.GetValueOrDefault(0)) / (1 + percent);
                var a = (spandate * item.TotalMoney / (1 + percent) - (TotalAmountAccepted != null ? TotalAmountAccepted : 0));
                if (a == null) a = 0;
                model.Unfinished = Convert.ToDecimal(a);
                model.UnfinishedText = _priceFormatter.FormatPrice(Convert.ToDecimal(a), true, item.currency);
            }
            //model.Unfinished;
        }

        public TaskModel PrepareDelete(int TaskId)
        {
            var item = _workTaskService.GetTaskById(TaskId);
            _workTaskService.DeleteTask(item);
            InsertContractLogDelete(item);
            return item.ToModel<TaskModel>();
        }
        #endregion
        #region taskLog 
        public void InsertTaskLog(TaskLogModel item)
        {
            var task = _workTaskService.GetTaskById(item.TaskId);

            var Tasklog = new TaskLog
            {
                CreatedDate = DateTime.Now,
                CreatorId = _workContext.CurrentCustomer.Id,
                TaskData = task.ToModel<TaskModel>().toStringJson(),
                TaskId = item.TaskId,
                Note = item.Note
            };
            _workTaskService.InsertTaskLog(Tasklog);
        }
        #endregion
        #region contractlog
        public void InsertContractLog(Task item)
        {
            var contract = _contractService.GetContractById(item.ContractId);
            var task = _workTaskService.GetTaskById(item.Id);
            var contractlog = new ContractLog();
            contractlog.ContractId = contract.Id;
            contractlog.CreatedDate = DateTime.Now;
            contractlog.CreatorId = _workContext.CurrentCustomer.Id;
            contractlog.ContractData = task.ToModel<TaskModel>().toStringJson();
            contractlog.PeriodId = contract.PeriodId;
            _contractLogService.InsertContractLog(contractlog, "Tạo mới công việc " + item.Name);
        }
        public void InsertContractLogDelete(Task item)
        {
            var contract = _contractService.GetContractById(item.ContractId);
            var task = _workTaskService.GetTaskById(item.Id);
            var contractlog = new ContractLog();
            contractlog.ContractId = contract.Id;
            contractlog.CreatedDate = DateTime.Now;
            contractlog.CreatorId = _workContext.CurrentCustomer.Id;
            contractlog.ContractData = task.ToModel<TaskModel>().toStringJson();
            contractlog.PeriodId = contract.PeriodId;
            _contractLogService.InsertContractLog(contractlog, "Xoá công việc " + item.Name);
        }
        public bool CheckTaskEndDate(TaskModel model)
        {
            if ((model.EndDate == null) || (model.StartDate == null))
                return true;
            if (  (model.EndDate > model.ContractEndDate) || (model.EndDate < model.ContractStarDate) || (model.EndDate < model.StartDate))
                return false;
            return true;
        }
        public bool CheckTaskStartDate(TaskModel model)
        {
            if ((model.EndDate == null) || (model.StartDate == null))
                return true;
            if ((model.StartDate > model.EndDate) || (model.StartDate < model.ContractStarDate) || (model.StartDate > model.ContractEndDate))
                return false;
            return true;
        }
        #endregion
        #region ContractAcceptanceTasks
        public ContractAcceptanceListModel PrepareListContractAcceptanceTasks(ContractAcceptanceSearchModel searchmodel)
        {
            var items = _contractService.getAllContractAcceptance(searchmodel.ContractId, searchmodel.TaskId).ToList();

            return new ContractAcceptanceListModel
            {
                Data = items.Select(c => c.ToModel<ContractAcceptanceModel>()).ToList(),
                Total = items.Count
            };
        }

        public TaskListModel PrepareListTaskContractAcceptances(TaskSearchModel searchModel)
        {
            var items = _workTaskService.getallTaskHasContractAcceptance(searchModel.ContractId).ToList();
            return new TaskListModel
            {
                Data = items.Select(c => c.ToModel<TaskModel>()),
                Total = items.Count
            };
        }
        #endregion
        #region TaskResource
        public void PrepareTaskResource(TaskModel model)
        {
            var taskResources = _workTaskService.GetTaskResources(model.Id);
            //lay danh sach nguoi dung tren task
            model.taskResourceModels = taskResources.GroupBy(c => c.CustomerId)
              .Select(group => group.First()).Select(c =>
              {
                  var m = new TaskResourceModel
                  {
                      TaskId = c.TaskId,
                      CustomerId = c.CustomerId,
                      CustomerFullName = _customerService.GetCustomerFullName(c.customer),
                      CustomerAvatarUrl = GetCustomerAvatarUrl(c.customer),
                      UnitId = c.UnitId,
                      UnitName = c.unitInfo != null ? c.unitInfo.Name : ""
                  };
                  return m;
              }).Distinct().ToList();
            //tao thong tin role id cho resource model
            //1 user co the co nhieu role 
            foreach (var m in model.taskResourceModels)
            {
                m.RoleModels = taskResources.Where(c => c.TaskId == m.TaskId
                    && c.CustomerId == m.CustomerId).Select(x => x.contractRole.ToModel<CustomerRoleModel>()).ToList();
                m.RoleIds = m.RoleModels.Select(c => c.Id).ToArray();
            }
        }
        #endregion
    }
}
