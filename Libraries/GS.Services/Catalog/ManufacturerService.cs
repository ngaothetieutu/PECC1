using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Security;
using GS.Core.Domain.Stores;
using GS.Services.Events;

namespace GS.Services.Catalog
{
    /// <summary>
    /// Manufacturer service
    /// </summary>
    public partial class ManufacturerService : IManufacturerService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public ManufacturerService(CatalogSettings catalogSettings,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<AclRecord> aclRepository,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            this._catalogSettings = catalogSettings;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._aclRepository = aclRepository;
            this._manufacturerRepository = manufacturerRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._storeContext = storeContext;
            this._workContext = workContext;
            this._entityName = typeof(Manufacturer).Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void DeleteManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            manufacturer.Deleted = true;
            UpdateManufacturer(manufacturer);

            //event notification
            _eventPublisher.EntityDeleted(manufacturer);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="manufacturerName">Manufacturer name</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturers</returns>
        public virtual IPagedList<Manufacturer> GetAllManufacturers(string manufacturerName = "",
            int storeId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _manufacturerRepository.Table;
            if (!showHidden)
                query = query.Where(m => m.Published);
            if (!string.IsNullOrWhiteSpace(manufacturerName))
                query = query.Where(m => m.Name.Contains(manufacturerName));
            query = query.Where(m => !m.Deleted);
            query = query.OrderBy(m => m.DisplayOrder).ThenBy(m => m.Id);

            if ((storeId <= 0 || _catalogSettings.IgnoreStoreLimitations) && (showHidden || _catalogSettings.IgnoreAcl))
                return new PagedList<Manufacturer>(query, pageIndex, pageSize);
            
            if (!showHidden && !_catalogSettings.IgnoreAcl)
            {
                //ACL (access control list)
                var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                query = from m in query
                    join acl in _aclRepository.Table
                        on new { c1 = m.Id, c2 = _entityName } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into m_acl
                    from acl in m_acl.DefaultIfEmpty()
                    where !m.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                    select m;
            }

            if (storeId > 0 && !_catalogSettings.IgnoreStoreLimitations)
            {
                //Store mapping
                query = from m in query
                    join sm in _storeMappingRepository.Table
                        on new { c1 = m.Id, c2 = _entityName } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into m_sm
                    from sm in m_sm.DefaultIfEmpty()
                    where !m.LimitedToStores || storeId == sm.StoreId
                    select m;
            }

            query = query.Distinct().OrderBy(m => m.DisplayOrder).ThenBy(m => m.Id);
            
            return new PagedList<Manufacturer>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public virtual Manufacturer GetManufacturerById(int manufacturerId)
        {
            if (manufacturerId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.ManufacturersByIdCacheKey, manufacturerId);
            return _cacheManager.Get(key, () => _manufacturerRepository.GetById(manufacturerId));
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            _manufacturerRepository.Insert(manufacturer);

            //cache
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ManufacturersPatternCacheKey);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductManufacturersPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(manufacturer);
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public virtual void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException(nameof(manufacturer));

            _manufacturerRepository.Update(manufacturer);

            //cache
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ManufacturersPatternCacheKey);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProductManufacturersPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(manufacturer);
        }

        

        /// <summary>
        /// Returns a list of names of not existing manufacturers
        /// </summary>
        /// <param name="manufacturerIdsNames">The names and/or IDs of the manufacturers to check</param>
        /// <returns>List of names and/or IDs not existing manufacturers</returns>
        public virtual string[] GetNotExistingManufacturers(string[] manufacturerIdsNames)
        {
            if (manufacturerIdsNames == null)
                throw new ArgumentNullException(nameof(manufacturerIdsNames));

            var query = _manufacturerRepository.Table;
            var queryFilter = manufacturerIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = query.Select(m => m.Name).Where(m => queryFilter.Contains(m)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any()) 
                return queryFilter.ToArray();

            //filtering by IDs
            filter = query.Select(c => c.Id.ToString()).Where(c => queryFilter.Contains(c)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        
        #endregion
    }
}