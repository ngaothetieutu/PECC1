using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Catalog;
using GS.Services.Events;

namespace GS.Services.Catalog
{
    /// <summary>
    /// Review type service implementation
    /// </summary>
    public partial class ReviewTypeService : IReviewTypeService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ReviewType> _reviewTypeRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ReviewTypeService(IEventPublisher eventPublisher,
            IRepository<ReviewType> reviewTypeRepository,
            IStaticCacheManager cacheManager)
        {
            this._eventPublisher = eventPublisher;
            this._reviewTypeRepository = reviewTypeRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        #region Review type

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<ReviewType> GetAllReviewTypes()
        {
            return _cacheManager.Get(GSCatalogDefaults.ReviewTypeAllKey, () =>
            {
                return _reviewTypeRepository.Table
                    .OrderBy(reviewType => reviewType.DisplayOrder).ThenBy(reviewType => reviewType.Id)
                    .ToList();
            });
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="reviewTypeId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ReviewType GetReviewTypeById(int reviewTypeId)
        {
            if (reviewTypeId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.ReviewTypeByIdKey, reviewTypeId);
            return _cacheManager.Get(key, () => _reviewTypeRepository.GetById(reviewTypeId));
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void InsertReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Insert(reviewType);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ReviewTypeByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(reviewType);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void UpdateReviewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Update(reviewType);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ReviewTypeByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(reviewType);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        public virtual void DeleteReiewType(ReviewType reviewType)
        {
            if (reviewType == null)
                throw new ArgumentNullException(nameof(reviewType));

            _reviewTypeRepository.Delete(reviewType);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ReviewTypeByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(reviewType);
        }

        #endregion

        

        #endregion
    }
}