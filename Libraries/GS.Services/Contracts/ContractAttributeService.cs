using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Services.Events;

namespace GS.Services.Contracts
{
    public partial class ContractAttributeService:IContractAttributeService
    {
        #region Fields
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ContractAttribute> _contractAttributeRepository;
        private readonly IRepository<ContractAttributeValue> _contractAttributeValueRepository;
        #endregion

        #region Ctor
        public ContractAttributeService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<ContractAttribute> contractAttributeRepository,
            IRepository<ContractAttributeValue> contractAttributeValueRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._contractAttributeRepository = contractAttributeRepository;
            this._contractAttributeValueRepository = contractAttributeValueRepository;
        }
        #endregion

        #region Methods
        public virtual void DeleteContractAttribute(ContractAttribute contractAttribute)
        {
            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            _contractAttributeRepository.Delete(contractAttribute);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityDeleted(contractAttribute);
        }

        public virtual IList<ContractAttribute> GetAllContractAttributes()
        {
            return _cacheManager.Get(GSContractServiceDefaults.ContractAttributesAllCacheKey, () =>
            {
                var query = from ca in _contractAttributeRepository.Table
                            orderby ca.DisplayOrder, ca.Id
                            select ca;
                return query.ToList();
            });            
        }

        public virtual ContractAttribute GetContractAttributeById(int contractAttributeId)
        {
            if (contractAttributeId == 0)
                return null;

            var key = string.Format(GSContractServiceDefaults.ContractAttributesByIdCacheKey, contractAttributeId);
            return _cacheManager.Get(key, () => _contractAttributeRepository.GetById(contractAttributeId));
        }

        public virtual void InsertContractAttribute(ContractAttribute contractAttribute)
        {
            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            _contractAttributeRepository.Insert(contractAttribute);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityInserted(contractAttribute);
        }

        public virtual void UpdateContractAttribute(ContractAttribute contractAttribute)
        {
            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            _contractAttributeRepository.Update(contractAttribute);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityUpdated(contractAttribute);
        }

        public virtual void DeleteContractAttributeValue(ContractAttributeValue contractAttributeValue)
        {
            if (contractAttributeValue == null)
                throw new ArgumentNullException(nameof(contractAttributeValue));

            _contractAttributeValueRepository.Delete(contractAttributeValue);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityDeleted(contractAttributeValue);
        }
        
        public virtual void InsertContractAttributeValue(ContractAttributeValue contractAttributeValue)
        {
            if (contractAttributeValue == null)
                throw new ArgumentNullException(nameof(contractAttributeValue));

            _contractAttributeValueRepository.Insert(contractAttributeValue);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityDeleted(contractAttributeValue);
        }

        public virtual void UpdateContractAttributeValue(ContractAttributeValue contractAttributeValue)
        {
            if (contractAttributeValue == null)
                throw new ArgumentNullException(nameof(contractAttributeValue));

            _contractAttributeValueRepository.Update(contractAttributeValue);

            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSContractServiceDefaults.ContractAttributeValuesPatternCacheKey);

            _eventPublisher.EntityUpdated(contractAttributeValue);
        }

        public virtual IList<ContractAttributeValue> GetContractAttributeValues(int contractAttributeId)
        {
            var key = string.Format(GSContractServiceDefaults.ContractAttributeValuesAllCacheKey, contractAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from cav in _contractAttributeValueRepository.Table
                            orderby cav.DisplayOrder, cav.Id
                            where cav.ContractAttributeId == contractAttributeId
                            select cav;
                var contractAttributeValues = query.ToList();
                return contractAttributeValues;
            });
        }

        public virtual ContractAttributeValue GetContractAttributeValueById(int contractAttributeValueId)
        {
            if (contractAttributeValueId == 0)
                return null;

            var key = string.Format(GSContractServiceDefaults.ContractAttributeValuesByIdCacheKey, contractAttributeValueId);
            return _cacheManager.Get(key, () => _contractAttributeValueRepository.GetById(contractAttributeValueId));
        }
        #endregion
    }
}
