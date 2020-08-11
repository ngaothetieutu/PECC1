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
    public class ConstructionTypeService: IConstructionTypeService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ConstructionType> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ConstructionTypeService(IEventPublisher eventPublisher,
            IRepository<ConstructionType> itemRepository,
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
        public virtual IList<ConstructionType> GetAllConstructionTypes()
        {
            return _cacheManager.Get(GSCatalogDefaults.ConstructionTypeAllKey, () =>
            {
                return _itemRepository.Table
                    .OrderBy(item => item.Name)
                    .ToList();
            });
        }

        public virtual IList<ConstructionType> GetConstructionTypeByName(string Name = "")
        {
            var query = _itemRepository.Table;
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name) && c.Id != 1000);
            return query
                   .OrderBy(item => item.Name)
                   .ToList();
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ConstructionType GetConstructionTypeById(int itemId)
        {
            if (itemId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.ConstructionTypeByIdKey, itemId);
            return _cacheManager.Get(key, () => _itemRepository.GetById(itemId));
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertConstructionType(ConstructionType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionTypeByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateConstructionType(ConstructionType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionTypeByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteConstructionType(ConstructionType item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionTypeByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }

        #endregion



        #endregion
    }
}
