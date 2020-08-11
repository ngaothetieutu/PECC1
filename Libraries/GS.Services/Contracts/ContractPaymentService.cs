using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GS.Services.Contracts
{
    public class ContractPaymentService : IContractPaymentService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IRepository<ContractPayment> _itemRepository;
        private readonly IRepository<ContractJoint> _contractJointRepository;
        private readonly IRepository<ContractFile> _contractFileRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IRepository<ContractView> _contractViewRepository;
        private readonly IRepository<Task> _taskRepository;
        #endregion

        #region Ctor

        public ContractPaymentService(IEventPublisher eventPublisher,
            IProcuringAgencyService procuringAgencyService,

            IRepository<ContractPayment> itemRepository,
            IRepository<ContractJoint> contractJointRepository,
            IRepository<ContractFile> contractFileRepository,
            IStaticCacheManager cacheManager,
            IWorkContext workContext,
            IRepository<ContractView> contractViewRepository)
        {
            this._eventPublisher = eventPublisher;
            this._procuringAgencyService = procuringAgencyService;
            this._itemRepository = itemRepository;
            this._contractJointRepository = contractJointRepository;
            this._contractFileRepository = contractFileRepository;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
            this._contractViewRepository = contractViewRepository;
        }
        #endregion
        #region Review type
        /// <summary>
        /// Delete ContractPayment
        /// </summary>
        /// <param name="item">Review type</param>
        public void DeleteContractPayment(ContractPayment item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.StatusId = 4;
            _itemRepository.Update(item);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }
        /// <summary>
        /// Gets all ContractPayment
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ContractPayment> GetAllContractsPayment(int ContractId = 0, int AcceptanceId = 0, int TaskId = 0, int TypeId = 0, int IsReceipts = 0)
        {
            //chu y: tat ca giao dich tien phai co trang thai khác hủy
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (AcceptanceId > 0)
            {
                query = query.Where(c => c.AcceptanceId == AcceptanceId);
            }
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (IsReceipts == 1)
            {
                query = query.Where(c => c.IsReceipts == true);
            }
            if (IsReceipts == 2)
            {
                query = query.Where(c => c.IsReceipts == false);
            }
            return query.OrderByDescending(item => item.Id).ToList();
        }
        public IPagedList<ContractPayment> GetContractPayments(int ContractId = 0, int pageIndex = 0, int pageSize = int.MaxValue, int taskId = 0, List<int> SelectedListTaskIds = null, string KeySearch = null, int TypeId = 0, int IsReceipts = 0)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (!string.IsNullOrEmpty(KeySearch))
            {
                query = query.Where(c => c.Code.Contains(KeySearch) || c.Description.Contains(KeySearch));
            }
            if (taskId > 0)
            {
                query = query.Where(c => c.TaskId == taskId);
            }
            if (SelectedListTaskIds != null && SelectedListTaskIds.Count > 0)
            {
                query = query.Where(c => SelectedListTaskIds.Contains((int)c.TaskId));
            }
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (IsReceipts == 1)
            {
                query = query.Where(c => c.IsReceipts == true);
            }
            if (IsReceipts == 2)
            {
                query = query.Where(c => c.IsReceipts == false);
            }
            query = query.OrderByDescending(c => c.CreatedDate);
            return new PagedList<ContractPayment>(query, pageIndex, pageSize);
        }
        public IList<ContractPayment> GetAllContractPaymentsByPaymentRequestId(int paymentRequestId)
        {
            return _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy && c.PaymentRequestId == paymentRequestId).ToList();
        }
        public IList<ContractPayment> GetAllContractPaymentsByAcceptanceId(int acceptanceId)
        {
            return _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy && c.AcceptanceId == acceptanceId).ToList();
        }
        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        public ContractPayment GetContractPaymentById(int Id)
        {
            return _itemRepository.GetById(Id);
        }
        /// <summary>
        /// Gets a customer by GUIDs
        /// </summary>
        /// <param name="contractGuid"></param>
        /// <returns></returns>
        public virtual ContractPayment GetContractPaymentByGuid(Guid paymentGuid)
        {
            if (paymentGuid == Guid.Empty)
                return null;

            var query = from c in _itemRepository.Table
                        where c.PaymentGuid == paymentGuid
                        orderby c.Id
                        select c;
            var payment = query.FirstOrDefault();
            return payment;
        }


        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        public void InsertContractPayment(ContractPayment item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        public void UpdateContractPayment(ContractPayment item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public virtual decimal GetAllContractsPaymentAmountByPaymentRequestId(int PaymentRequestId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy && c.PaymentRequestId == PaymentRequestId);

            return query.Sum(c => c.Amount);
        }
        public virtual decimal GetAllContractsPaymentAmountByAcceptanceId(int AcceptanceId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy && c.AcceptanceId == AcceptanceId);

            return query.Sum(c => c.Amount);
        }
        public virtual decimal GetAllContractsPaymentAmountByTaskId(int TaskId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy 
            && c.TaskId == TaskId
            && c.IsReceipts == false);
            return query.Sum(c => c.Amount);
        }

        public IList<ContractPayment> GetAllContractPaymentsByExpenditureId(int ExpenditureId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
            && c.ExpenditureId == ExpenditureId);
            return query.ToList();
        }
        public void DeleteContractPaymentsByExpenditureId(int ExpenditureId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
            && c.ExpenditureId == ExpenditureId);
            _itemRepository.Delete(query);
        }
        #endregion
    }
}
