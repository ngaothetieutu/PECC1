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
    public class ConstructionCapitalService : IConstructionCapitalService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ConstructionCapital> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;
        #endregion
        #region Ctor
        public ConstructionCapitalService(IEventPublisher eventPublisher,
            IRepository<ConstructionCapital> itemRepository,
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
        public virtual IList<ConstructionCapital> GetAllConstructionCapital()
        {
            return _cacheManager.Get(GSCatalogDefaults.ConstructionCapitalAllKey, () =>
            {
                return _itemRepository.Table
                    .OrderBy(item => item.Name)
                    .ToList();
            });
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ConstructionCapital GetConstructionCapitalById(int itemId)
        {
            if (itemId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.ConstructionCapitalByIdKey, itemId);
            return _cacheManager.Get(key, () => _itemRepository.GetById(itemId));
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertConstructionCapital(ConstructionCapital item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionCapitalByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateConstructionCapital(ConstructionCapital item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionCapitalByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteConstructionCapital(ConstructionCapital item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionCapitalByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }

        public virtual IList<ConstructionCapital> GetConstructionCapitalsByName(string Name = "")
        {
            var query = _itemRepository.Table;
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name));
            return query
                   .OrderBy(item => item.Name)
                   .ToList();
        }
        #endregion

        #endregion
    }
}
