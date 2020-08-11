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
    public class ConstructionService : IConstructionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Construction> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<Contract> _contractRepository;

        #endregion

        #region Ctor

        public ConstructionService(IEventPublisher eventPublisher,
            IRepository<Construction> itemRepository,
            IRepository<Contract> contractRepository,
            IStaticCacheManager cacheManager)
        {
            this._contractRepository = contractRepository;
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
        public virtual IList<Construction> GetAllConstructions()
        {
            return _cacheManager.Get(GSCatalogDefaults.ConstructionAllKey, () =>
            {
                return _itemRepository.Table
                   .OrderBy(item => item.Name)
                   .ToList();
            });
           
        }

        public IPagedList<Construction> GetContructionByName(int TypeId = 0, string Name = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _itemRepository.Table;
            if (TypeId > 0)
                query = query.Where(c => c.TypeId == TypeId);
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name) || c.Code.Contains(Name));
            return new PagedList<Construction>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual Construction GetConstructionById(int itemId)
        {
            if (itemId == 0)
                return null;
            return _itemRepository.GetById(itemId);
            //var key = string.Format(GSCatalogDefaults.ConstructionByIdKey, itemId);
            //return _cacheManager.Get(key, () => _itemRepository.GetById(itemId));
            
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertConstruction(Construction item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateConstruction(Construction item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteConstruction(Construction item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ConstructionByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }

        public virtual Decimal GetTotalConstruction(int year = 0)
        {
            var query = _itemRepository.Table;
            if (year != 0)
            {
                var queryContract = _contractRepository.Table.Where(c => Convert.ToDateTime(c.StartDate).Year < year)
                    .Select(c => c.ConstructionId).Distinct();
                query = query.Where(c => !queryContract.Contains(c.Id));
            }
            return query.Count();
        }

        #endregion

        #endregion
    }
}
