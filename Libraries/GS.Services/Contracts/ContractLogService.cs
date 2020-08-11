using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Services.Catalog;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GS.Services.Contracts
{
    public class ContractLogService : IContractLogService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ContractLog> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IContractService _contractService;

        #endregion
        #region Ctor

        public ContractLogService(IEventPublisher eventPublisher,
            IContractService contractService,
            IWorkContext workContext,
            IRepository<ContractLog> itemRepository,
            IStaticCacheManager cacheManager)
        {
            this._eventPublisher = eventPublisher;
            this._contractService = contractService;
            this._workContext = workContext;
            this._itemRepository = itemRepository;
            this._cacheManager = cacheManager;
        }
        #endregion
        #region Methods

        #region Review type

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ContractLog> GetAllContractLogs()
        {
            return _itemRepository.Table
                   .OrderBy(item => item.Id)
                   .ToList();
        }
        /// <summary>
        /// Gets all contractlog by ContractId
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ContractLog> GetAllContractLogsByContractId(int contractId)
        {
            return _itemRepository.Table
                   .Where(item => item.ContractId == contractId)
                   .OrderByDescending(item => item.Id)
                   .ToList();
        }

        public IPagedList<ContractLog> GetContractLogByContractIdorName(int ContractID = 0, string Note = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _itemRepository.Table;
            if (ContractID > 0)
                query = query.Where(c => c.ContractId == ContractID)
                    .OrderByDescending(c => c.CreatedDate);

            if (!string.IsNullOrEmpty(Note))
                query = query.Where(c => c.Note.Contains(Note));

            return new PagedList<ContractLog>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ContractLog GetContractLogById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertContractLog(ContractLog item, string note)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.Note = note;
            _itemRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }

        #endregion

        #endregion
    }
}
