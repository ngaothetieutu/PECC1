using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using GS.Services.Directory;
using GS.Services.Events;

namespace GS.Services.Works
{
    public class WorkTaskService : IWorkTaskService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Task> _itemRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IRepository<TaskLog> _taskLogRepository;
        private readonly IRepository<ContractAcceptanceTaskMapping> _ContractAcceptanceTaskMappingRepository;
        private readonly IRepository<TaskResource> _taskResourceRepository;
        private readonly IRepository<ContractPaymentAcceptanceSub> _contractPaymentAcceptanceSubRepository;
        private readonly IRepository<ContractPaymentAcceptance> _contractPaymentAcceptanceRepository;
        private readonly IRepository<TaskContract> _taskContractRepository;
        private readonly CurrencySettings _currencySettings;
        private readonly ICurrencyService _currencyService;
        #endregion

        #region Ctor

        public WorkTaskService(CurrencySettings currencySettings,
            ICurrencyService currencyService,
            IEventPublisher eventPublisher,
            IRepository<TaskResource> taskResourceRepository,
            IRepository<Task> itemRepository,
            IRepository<Contract> contractRepository,
            IWorkContext workContext,
            IStaticCacheManager cacheManager,
            IRepository<TaskLog> taskLogRepository,
            IRepository<ContractAcceptanceTaskMapping> contractAcceptanceTaskMappingRepository,
            IRepository<ContractPaymentAcceptanceSub> contractPaymentAcceptanceSubRepository,
            IRepository<ContractPaymentAcceptance> contractPaymentAcceptanceRepository,
            IRepository<TaskContract> taskContractRepository)
        {
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._taskResourceRepository = taskResourceRepository;
            this._eventPublisher = eventPublisher;
            this._contractRepository = contractRepository;
            this._itemRepository = itemRepository;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
            this._taskLogRepository = taskLogRepository;
            this._ContractAcceptanceTaskMappingRepository = contractAcceptanceTaskMappingRepository;
            this._taskContractRepository = taskContractRepository;
            this._contractPaymentAcceptanceSubRepository = contractPaymentAcceptanceSubRepository;
            this._contractPaymentAcceptanceRepository = contractPaymentAcceptanceRepository;
        }
        #endregion
        #region Method
        public void DeleteTask(Task item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.taskStatus = TaskStatus.Destroy;
            _itemRepository.Update(item);
            if (item.ParentId == null)
            {
                //Delete TaskContract
                var taskContract = GetTaskContractByTaskId(item.Id);
                if (taskContract != null)
                {
                    deleteTaskContract(taskContract);
                }
            }
            //event notification
            _eventPublisher.EntityUpdated(item);
            //cap nhat lai amount
            UpdateAmountTotal(item);
        }

        public IList<Task> GetAllTasks()
        {
            return _itemRepository.Table.Where(c=>c.taskStatus != TaskStatus.Destroy)
                .OrderBy(item => item.Id)
                .ToList();
        }
        public IList<Task> GetAllTasksByContractDestroyId(int ContractId)
        {
            var query = _itemRepository.Table.Where(c => c.ContractId == ContractId);
            return query.ToList();
        }
        public Task GetTaskById(int itemId)
        {
            if (itemId == 0)
                return null;
            return _itemRepository.GetById(itemId);
        }
        public Task GetTaskByTaskProcuringAgencyId(int TaskProcuringAgencyId)
        {
            if (TaskProcuringAgencyId == 0)
                return null;
            return _itemRepository.Table.Where(c =>c.StatusId != (int)TaskStatus.Destroy && c.TaskProcuringAgencyId == TaskProcuringAgencyId).FirstOrDefault();
        }
        public IList<Task> getTasksByConTractId(int ContractId = 0,int ParentId = 0, bool includParent = false, bool isAll=false, int ContractTypeId = 0)
        {
            var query = _itemRepository.Table.Where(c=>c.StatusId != (int)TaskStatus.Destroy);
            if (ContractId >0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if(ParentId == 0 && !isAll)
            {
                query = query.Where(c => c.ParentId == null);
            }
            if (ContractTypeId >0)
            {
                query = query.Where(c => c.ContractTypeId == ContractTypeId);
            }
            if(ParentId > 0)
            {
                if (includParent)
                {
                    query = query.Where(c => c.ParentId == ParentId || c.Id == ParentId);
                }
                else
                {
                    query = query.Where(c => c.ParentId == ParentId);
                }  
            }
            return query.OrderBy(item => item.Id).ToList();
        }
        public IList<Task> getTaskByContractIdForContractSettlement(int ContractId = 0)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)TaskStatus.Destroy
            && c.TreeLevel == 1 && c.ContractId == ContractId);

            return query.OrderBy(item => item.Id).ToList();
        }
       
        public void InsertTask(Task item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
                           
            _itemRepository.Insert(item);           
            //event notification
            _eventPublisher.EntityInserted(item);
            UpdateTaskComplete(item);
            UpdateAmountTotal(item);
        }

        public void UpdateTask(Task item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Update(item);           
            //event notification
            _eventPublisher.EntityUpdated(item);
            UpdateTaskComplete(item);
            UpdateAmountTotal(item);
        }
        public int CountTaskChild(int TaskId)
        {
            return  _itemRepository.Table.Where( c => c.StatusId != (int)TaskStatus.Destroy && c.ParentId == TaskId).Count();
        }
        public int CountTask(int CustomerId, TaskStatus taskStatus = TaskStatus.All)
        {
            var query = _itemRepository.Table;
            if (taskStatus == TaskStatus.All)
                query = query.Where(c => c.StatusId != (int)TaskStatus.Destroy);
            else
                query = query.Where(c => c.StatusId != (int)taskStatus);
            return query.Count();
        }

        public void UpdateTaskComplete(Task item)
        {
            var code = item.Id.toCode(7);
            item.Code = code;
            item.TreeNode = code;
            //update Code
            if (item.ParentId >0)
            {
                //lay thong tin ma cha
                var taskcha = GetTaskById((int)item.ParentId);
                item.TreeNode = taskcha.TreeNode + "-" + item.Code;
                item.TreeLevel = taskcha.TreeLevel +1;
            }       
           _itemRepository.Update(item);
        }
        public IList<Task> getAllTaskbyUnit(int UnitId , int ContractId = 0)
        {
            var query = _itemRepository.Table.Where(c =>c.UnitId ==UnitId && c.StatusId != (int)TaskStatus.Destroy);
            if (ContractId >0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.ToList();
        }
        /// <summary>
        /// lay danh sach cong viec thuoc don vi cua hop dong co nghiem thu thanh toan trong khoang thoi gian
        /// </summary>
        /// <param name="AprovalDate, UnitId, ContractId"></param>
        public IList<Task> GetAllTasksByAprovalDatePaymentAcceptance(int unitId, int ContractId, string approvalDateString)
        {
            var fromDate = new DateTime();
            var approvalDate =  DateTime.ParseExact(approvalDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var year = approvalDate.Year;
            if (approvalDate.Month >= 1 && approvalDate.Month <= 3)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0101", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvalDate.Month >= 4 && approvalDate.Month <= 6)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0401", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvalDate.Month >= 7 && approvalDate.Month <= 9)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0701", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvalDate.Month >9)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "1001", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            var query = _contractPaymentAcceptanceSubRepository.Table.Where(c => c.StatusId != (int)PaymentAcceptanceSubStatus.Destroy
            && c.ContractId == ContractId && c.UnitId == unitId
            && c.contractPaymentAcceptance.ApprovalDate >= fromDate && c.contractPaymentAcceptance.ApprovalDate <= approvalDate)
            .GroupBy(c => c.TaskId).Select(gruop => gruop.First()).Select(c=> c.task);
            return query.ToList();
        }
        
        /// <summary>
        /// Cap nhat lai gia tri cho task khi co su thay doi
        /// </summary>
        /// <param name="TaskId"></param>
        public void UpdateTaskAmount(int TaskId)
        {
            var item = GetTaskById(TaskId);
            if(item!=null)
            {
                item.TotalMoney = _itemRepository.Table.Where(c => c.ContractId == item.ContractId && c.ParentId == TaskId).Sum(x => x.TotalMoney);
                _itemRepository.Update(item);
                UpdateAmountTotal(item);
            }
        }
        public IList<ContractCurrency> GetContractCurrencies(int ContractId)
        {
            //lay tat ca cong viec cap 1 khong co lien danh
            var _tasks = _itemRepository.Table.Where(c => c.StatusId != (int)TaskStatus.Destroy && c.ContractId == ContractId && c.ParentId == null && c.TaskProcuringAgencyId == null)
                    .ToList();

            var _currencies = _tasks
                    .Select(t => new
                    {
                        t.CurrencyId,
                        t.TotalMoney
                    })
                    .GroupBy(g => g.CurrencyId)
                    .Select(x =>
                    {
                        var _contractCurrency = new ContractCurrency();
                        _contractCurrency.ContractId = ContractId;                        
                        _contractCurrency.CurrencyId = x.Key.GetValueOrDefault(0);
                        if(_contractCurrency.CurrencyId>0)
                        {
                            _contractCurrency.currency = _currencyService.GetCurrencyById(_contractCurrency.CurrencyId);
                        }
                        _contractCurrency.TotalAmount = x.Sum(s => s.TotalMoney).GetValueOrDefault(0);
                        return _contractCurrency;
                    })
                    .OrderBy(o => o.currency.DisplayOrder)
                    .ToList();

            return _currencies;
        }
        /// <summary>
        /// Ham update gia tri task cha va contract
        /// </summary>
        /// <param name="item"></param>
        private void UpdateAmountTotal(Task item)
        {
            //lay tat ca cha cua item hien tai
            if(item.ParentId>0)
            {
                //var parentItem = GetTaskById(item.ParentId.Value);
                //parentItem.TotalMoney = _itemRepository.Table.Where(c => c.StatusId!=(int)TaskStatus.Destroy && c.ContractId == item.ContractId && c.ParentId == parentItem.Id).Sum(x => x.TotalMoney);
                //_itemRepository.Update(parentItem);
                //UpdateAmountTotal(parentItem);
            }
            else
            {
                //ko con cha, update gia tri hop dong
                UpdateContractAmountSummary(item.ContractId);
            }
        }
        public void UpdateContractAmountSummary(int ContractId)
        {
            var contract = _contractRepository.GetById(ContractId);
            //lay tat ca hang muc cong viec ngoai cung cua hop dong, va group theo don vi tien
            var _contractCurrencys = GetContractCurrencies(ContractId);
            //vi hop dong co the co nhieu loai tien te khac nhau, nen tong gia tri hop dong o day chi co y nghia cua VND va theo gia                 
            contract.TotalAmount = _contractCurrencys.Sum(c => c.TotalAmount * c.currency.Rate);
            //tao thong tin mo ta cho gia tri hop dong
            contract.AmountSummary = _contractCurrencys.ToAmountSummary(_currencySettings.PrimaryExchangeRateCurrencyId);
            _contractRepository.Update(contract);
        }
        public IList<Task> GetTaskDeadLine(int contractId)
        {
            var query = _itemRepository.Table;
            var ls = query.Where(c => c.ParentId == null && c.ContractId == contractId && c.StatusId != (int)TaskStatus.Destroy)
                .OrderBy(c => c.EndDate).ToList();
            return ls;
        }
        public IList<Task> GetTaskbyListTaskIds(IList<int> ListTaskIds)
        {
            var query = _itemRepository.Table.Where(c=> c.StatusId != (int)TaskStatus.Destroy);
            if (ListTaskIds.Count>0)
            {
                query = query.Where(c => ListTaskIds.Contains(c.Id));
            }
            return query.ToList();
        }
        /// <summary>
        /// Danh sach cong viec lien danh cua hop dong
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public IList<Task> GetAllTaskContact(int contractId)
        {
            var query = _itemRepository.Table
                    .Where(c => c.StatusId != (int)TaskStatus.Destroy 
                    && c.ContractId == contractId
                    && c.ParentId == null
                    && c.TaskProcuringAgencyId != null);
            return query.ToList();
        }
        #endregion
        #region TaskLog
        public IList<TaskLog> GetAllTaskLogs()
        {
            return _taskLogRepository.Table.ToList();
        }

        public TaskLog GetTaskLogById(int Id)
        {
            return _taskLogRepository.GetById(Id);
        }

        public void InsertTaskLog(TaskLog item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _taskLogRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);            
        }


        #endregion
        #region ContractAcceptanceTask
        public IList<Task> getallTaskHasContractAcceptance(int ContractId,int ContractAcceptanceId = 0)
        {
            var query = _itemRepository.Table.Where(c=>c.ContractId==ContractId);
            //query = query.Join(_ContractAcceptanceTaskMappingRepository.Table, x => x.Id, y => y.TaskId,
            //           (x, y) => new { Tasks = x, Mapping = y })                   
            //       .Select(z => z.Tasks);
            if (ContractAcceptanceId > 0)
            {
                query= query.Join(_ContractAcceptanceTaskMappingRepository.Table, x => x.Id, y => y.TaskId,
                       (x, y) => new { Tasks = x, Mapping = y })
                       .Where(z => z.Mapping.ContractAcceptanceId == ContractAcceptanceId)
                   .Select(z => z.Tasks);
            }
            return query.ToList();            
        }
        #endregion
        #region Task Resource
        public virtual void InsertTaskResource(TaskResource item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.UnitId == 0)
            {
                item.UnitId = null;
            }
            _taskResourceRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public virtual void DeleteTaskResource(int ItemId)
        {
            var item = _taskResourceRepository.GetById(ItemId);
            if (item != null)
                _taskResourceRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public virtual IList<TaskResource> GetTaskResources(int TaskId, int CustomerId = 0)
        {
            var query = _taskResourceRepository.Table;
            if (TaskId > 0)
                query = query.Where(c => c.TaskId == TaskId);
            if (CustomerId > 0)
                query = query.Where(c => c.CustomerId == CustomerId);
            return query.ToList();
        }
        public virtual TaskResource GetTaskResourceById(int ItemId)
        {
            if (ItemId == 0)
                return null;

            return _taskResourceRepository.GetById(ItemId);
        }
        public virtual void DeleteListTaskResource(int TaskId, int CustomerId = 0)
        {
            var lst = GetTaskResources(TaskId, CustomerId).ToList();
            foreach (TaskResource Cr in lst)
            {
                DeleteTaskResource(Cr.Id);
            }
        }
        public virtual void UpdateTaskResource(TaskResource item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _taskResourceRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
        #region TaskContract
        public void InsertTaskContract(TaskContract item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if(_taskContractRepository.Table.Any(c=>c.TaskId==item.TaskId && c.ContractId==item.ContractId))
                {
                return;
            }
            _taskContractRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public void deleteTaskContract(TaskContract item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _taskContractRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }

        public IList<TaskContract> getTaskContract(int TaskId = 0, int ContractId = 0)
        {
            var query = _taskContractRepository.Table;
            if (TaskId >0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.ToList();
        }

        public TaskContract GetTaskContractById(int ItemId)
        {
            return _taskContractRepository.GetById(ItemId);
        }
        public IList<TaskContract> GetAllTaskContractByTaskId(int TaskId)
        {
            return _taskContractRepository.Table.Where(c=> c.TaskId == TaskId).ToList();
        }
        public TaskContract GetTaskContractByTaskId(int TaskId)
        {
            return  _taskContractRepository.Table.Where(c => c.TaskId == TaskId).FirstOrDefault();            
        }

        public TaskContract GetTaskContractByContractId(int ContractId)
        {
            return _taskContractRepository.Table.Where(c => c.ContractId == ContractId).FirstOrDefault();
        }
        public IList<TaskContract> GetListTaskContractByContractId(int ContractId)
        {
            return _taskContractRepository.Table.Where(c => c.ContractId == ContractId).ToList();
        }
        public IList<Task> GetAllTaskByContractIdAndStartdate(int ContractId)
        {
            return _itemRepository.Table.Where(c => c.ContractId == ContractId && c.ParentId == null && c.StartDate > DateTime.Now)
                .ToList();             
        }
        #endregion
    } 
}
