using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Services.Catalog;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GS.Services.Contracts
{
    public class ContractTypeService: IContractTypeService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ContractType> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<ContractContractTypeMapping> _contractTypeMappingRepository;

        #endregion

        #region Ctor

        public ContractTypeService(IEventPublisher eventPublisher,
            IRepository<ContractType> itemRepository,
            IStaticCacheManager cacheManager,
            IRepository<ContractContractTypeMapping> contractTypeMappingRepository)
        {
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._cacheManager = cacheManager;
            this._contractTypeMappingRepository = contractTypeMappingRepository;
        }

        #endregion

        #region Methods  contracttype

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ContractType> GetAllContractTypes(IList<int> Ids = null)
        {
            if (Ids != null && Ids.Count > 0)
            {
                var query = _itemRepository.Table.Where(c => Ids.Contains(c.Id) && c.Id != 1000);
                return query.ToList();
            }
            //lay tu cache
            return _cacheManager.Get(GSCatalogDefaults.ContractTypeAllKey, () =>
            {
                return _itemRepository.Table.Where(c=>c.Id != 1000)
                    .OrderBy(item => item.Name)
                    .ToList();
            });
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ContractType GetContractTypeById(int itemId)
        {
            if (itemId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.ContractTypeByIdKey, itemId);
            return _cacheManager.Get(key, () => _itemRepository.GetById(itemId));
        }
        /// <summary>
        /// Get ContractType by Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public virtual IList<ContractType> GetAllContractTypeByName(string Name = "")
        {
            var query = _itemRepository.Table.Where(c=>c.Id != 1000);
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name));
            return query
                   .OrderBy(item => item.Name)
                   .ToList();
        }
        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertContractType(ContractType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractTypeByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateContractType(ContractType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractTypeByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteContractType(ContractType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ContractTypeByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }

        #endregion
        #region contractType Mapping
        public IList<ContractContractTypeMapping> GetContractTypeMapping(int ContractId =0, int ContractTypeId=0)
        {
            var query = _contractTypeMappingRepository.Table;
            if (ContractId >0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (ContractTypeId > 0)
            {
                query = query.Where(c => c.ContractTypeId == ContractTypeId);
            }
            return query.ToList();
        }
        #endregion
    }
}
