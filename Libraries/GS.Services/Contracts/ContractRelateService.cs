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
   public class ContractRelateService : IContractRelateService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ContractRelate> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ContractRelateService(IEventPublisher eventPublisher,
            IRepository<ContractRelate> itemRepository,
            IStaticCacheManager cacheManager)
        {
            this._eventPublisher = eventPublisher;
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

        public virtual IList<ContractRelate> GetAllContractRelates()
        {
            return _cacheManager.Get(GSCatalogDefaults.ContractRelateAllKey, () =>
            {
                return _itemRepository.Table
                    .OrderBy(item => item.Contract1Id)
                    .ToList();
            });
        }
        public virtual IList<int> GetAllContractRelateIdByContractId(int ContractId)
        {
              var query =   _itemRepository.Table
                    .Where(item => item.Contract1Id == ContractId)
                    .Select(item => item.Contract2Id)
                    .ToList();
            return query;
        }
       
        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ContractRelate GetContractRelateById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }


        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertContractRelate(ContractRelate item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractRelateByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateContractRelate(ContractRelate item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractRelateByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteContractRelate(ContractRelate item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractRelateByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }
        public virtual ContractRelate GetContractRelateByContract2Id(int contract2Id)
        {
            if (contract2Id == 0)
                return null;

            return _itemRepository.Table
                .Where(item => item.Contract2Id == contract2Id)
                .FirstOrDefault();
        }
        #endregion

        #endregion
    }
}
