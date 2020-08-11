using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Advance;
using GS.Core.Domain.Works;
using GS.Services.Directory;
using GS.Services.Events;


namespace GS.Services.PaymentAdvance
{
    public class PaymentAdvanceService : IPaymentAdvanceService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ContractPaymentAdvance> _itemRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IRepository<TaskLog> _taskLogRepository;
        private readonly IRepository<TaskResource> _taskResourceRepository;
        private readonly IRepository<TaskContract> _taskContractRepository;
        private readonly CurrencySettings _currencySettings;
        private readonly ICurrencyService _currencyService;
        #endregion

        #region Ctor

        public PaymentAdvanceService(CurrencySettings currencySettings,
            ICurrencyService currencyService,
            IEventPublisher eventPublisher,
            IRepository<TaskResource> taskResourceRepository,
            IRepository<ContractPaymentAdvance> itemRepository,
            IRepository<Contract> contractRepository,
            IWorkContext workContext,
            IStaticCacheManager cacheManager,
            IRepository<TaskLog> taskLogRepository,         
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
            this._taskContractRepository = taskContractRepository;
        }
        #endregion
        #region PaymentAdvance
        public void deletePaymentAdvance(ContractPaymentAdvance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.StatusId = (int)PaymentAdvanceStatus.Delete;
            _itemRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }

        public IPagedList<ContractPaymentAdvance> getAllPaymentAdvance(int UnitId = 0, string Keysearch = "", int pageIndex = 0, int pageSize = int.MaxValue, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId == (int)PaymentAdvanceStatus.Use);
            if (UnitId >0)
            {
                query = query.Where(c => c.UnitId == UnitId);
            }
            if (!string.IsNullOrEmpty(Keysearch))
            {
                query = query.Where(c => c.Name.Contains(Keysearch) || c.Code.Contains(Keysearch));
            }
            if (FromDate.HasValue)
            {
                query = query.Where(c => c.PayDate > FromDate);
            }
            if (ToDate.HasValue)
            {
                query = query.Where(c => c.PayDate < ToDate);
            }
            return new PagedList<ContractPaymentAdvance>(query.OrderByDescending(c => c.Id), pageIndex, pageSize);
        }

        public ContractPaymentAdvance GetPaymentAdvanceById(int Id)
        {
            return _itemRepository.GetById(Id);
        }

        public void InsertPaymentAdvance(ContractPaymentAdvance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));           
            _itemRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public void UpdatePaymentAdvance(ContractPaymentAdvance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
           // item.CreatedDate = DateTime.Now;
            _itemRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public ContractPaymentAdvance GetContractAdvanceByGuid(Guid AdvanceGuid)
        {
            if (AdvanceGuid == Guid.Empty)
                return null;

            var query = from c in _itemRepository.Table
                        where c.AdvanceGuid == AdvanceGuid
                        orderby c.Id
                        select c;
            var advance = query.FirstOrDefault();
            return advance;
        }
        #endregion
    }
}
