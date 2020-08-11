using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Catalog;
using GS.Services.Events;

namespace GS.Services.Catalog
{
    /// <summary>
    /// Specification attribute service
    /// </summary>
    public partial class SpecificationAttributeService : ISpecificationAttributeService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;
        private readonly IRepository<SpecificationAttributeOption> _specificationAttributeOptionRepository;

        #endregion

        #region Ctor

        public SpecificationAttributeService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<SpecificationAttribute> specificationAttributeRepository,
            IRepository<SpecificationAttributeOption> specificationAttributeOptionRepository)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _specificationAttributeRepository = specificationAttributeRepository;
            _specificationAttributeOptionRepository = specificationAttributeOptionRepository;
        }

        #endregion

        #region Methods

        #region Specification attribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <returns>Specification attribute</returns>
        public virtual SpecificationAttribute GetSpecificationAttributeById(int specificationAttributeId)
        {
            if (specificationAttributeId == 0)
                return null;

            return _specificationAttributeRepository.GetById(specificationAttributeId);
        }

        /// <summary>
        /// Gets specification attributes
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Specification attributes</returns>
        public virtual IPagedList<SpecificationAttribute> GetSpecificationAttributes(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from sa in _specificationAttributeRepository.Table
                        orderby sa.DisplayOrder, sa.Id
                        select sa;
            var specificationAttributes = new PagedList<SpecificationAttribute>(query, pageIndex, pageSize);
            return specificationAttributes;
        }

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="specificationAttribute">The specification attribute</param>
        public virtual void DeleteSpecificationAttribute(SpecificationAttribute specificationAttribute)
        {
            if (specificationAttribute == null)
                throw new ArgumentNullException(nameof(specificationAttribute));

            _specificationAttributeRepository.Delete(specificationAttribute);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(specificationAttribute);
        }

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="specificationAttribute">The specification attribute</param>
        public virtual void InsertSpecificationAttribute(SpecificationAttribute specificationAttribute)
        {
            if (specificationAttribute == null)
                throw new ArgumentNullException(nameof(specificationAttribute));

            _specificationAttributeRepository.Insert(specificationAttribute);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(specificationAttribute);
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttribute">The specification attribute</param>
        public virtual void UpdateSpecificationAttribute(SpecificationAttribute specificationAttribute)
        {
            if (specificationAttribute == null)
                throw new ArgumentNullException(nameof(specificationAttribute));

            _specificationAttributeRepository.Update(specificationAttribute);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(specificationAttribute);
        }

        #endregion

        #region Specification attribute option

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <returns>Specification attribute option</returns>
        public virtual SpecificationAttributeOption GetSpecificationAttributeOptionById(int specificationAttributeOptionId)
        {
            if (specificationAttributeOptionId == 0)
                return null;

            return _specificationAttributeOptionRepository.GetById(specificationAttributeOptionId);
        }

        /// <summary>
        /// Get specification attribute options by identifiers
        /// </summary>
        /// <param name="specificationAttributeOptionIds">Identifiers</param>
        /// <returns>Specification attribute options</returns>
        public virtual IList<SpecificationAttributeOption> GetSpecificationAttributeOptionsByIds(int[] specificationAttributeOptionIds)
        {
            if (specificationAttributeOptionIds == null || specificationAttributeOptionIds.Length == 0)
                return new List<SpecificationAttributeOption>();

            var query = from sao in _specificationAttributeOptionRepository.Table
                        where specificationAttributeOptionIds.Contains(sao.Id)
                        select sao;
            var specificationAttributeOptions = query.ToList();
            //sort by passed identifiers
            var sortedSpecificationAttributeOptions = new List<SpecificationAttributeOption>();
            foreach (var id in specificationAttributeOptionIds)
            {
                var sao = specificationAttributeOptions.Find(x => x.Id == id);
                if (sao != null)
                    sortedSpecificationAttributeOptions.Add(sao);
            }

            return sortedSpecificationAttributeOptions;
        }

        /// <summary>
        /// Gets a specification attribute option by specification attribute id
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <returns>Specification attribute option</returns>
        public virtual IList<SpecificationAttributeOption> GetSpecificationAttributeOptionsBySpecificationAttribute(int specificationAttributeId)
        {
            var query = from sao in _specificationAttributeOptionRepository.Table
                        orderby sao.DisplayOrder, sao.Id
                        where sao.SpecificationAttributeId == specificationAttributeId
                        select sao;
            var specificationAttributeOptions = query.ToList();
            return specificationAttributeOptions;
        }

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOption">The specification attribute option</param>
        public virtual void DeleteSpecificationAttributeOption(SpecificationAttributeOption specificationAttributeOption)
        {
            if (specificationAttributeOption == null)
                throw new ArgumentNullException(nameof(specificationAttributeOption));

            _specificationAttributeOptionRepository.Delete(specificationAttributeOption);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(specificationAttributeOption);
        }

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOption">The specification attribute option</param>
        public virtual void InsertSpecificationAttributeOption(SpecificationAttributeOption specificationAttributeOption)
        {
            if (specificationAttributeOption == null)
                throw new ArgumentNullException(nameof(specificationAttributeOption));

            _specificationAttributeOptionRepository.Insert(specificationAttributeOption);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(specificationAttributeOption);
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeOption">The specification attribute option</param>
        public virtual void UpdateSpecificationAttributeOption(SpecificationAttributeOption specificationAttributeOption)
        {
            if (specificationAttributeOption == null)
                throw new ArgumentNullException(nameof(specificationAttributeOption));

            _specificationAttributeOptionRepository.Update(specificationAttributeOption);

            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductSpecificationAttributePatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(specificationAttributeOption);
        }

        /// <summary>
        /// Returns a list of IDs of not existing specification attribute options
        /// </summary>
        /// <param name="attributeOptionIds">The IDs of the attribute options to check</param>
        /// <returns>List of IDs not existing specification attribute options</returns>
        public virtual int[] GetNotExistingSpecificationAttributeOptions(int[] attributeOptionIds)
        {
            if (attributeOptionIds == null)
                throw new ArgumentNullException(nameof(attributeOptionIds));

            var query = _specificationAttributeOptionRepository.Table;
            var queryFilter = attributeOptionIds.Distinct().ToArray();
            var filter = query.Select(a => a.Id).Where(m => queryFilter.Contains(m)).ToList();
            return queryFilter.Except(filter).ToArray();
        }

        #endregion

    
        #endregion
    }
}