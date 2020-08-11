using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Contracts;
using GS.Services.Catalog;
using GS.Services.Events;
using GS.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GS.Core;

namespace GS.Services.Contracts
{
   public class ProcuringAgencyService : IProcuringAgencyService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProcuringAgency> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<ContractJoint> _contractJointRepository;
        #endregion

        #region Ctor

        public ProcuringAgencyService(IEventPublisher eventPublisher,
            IRepository<ContractJoint> contractJointRepository,
            IRepository<ProcuringAgency> itemRepository,
            IRepository<Contract> contractRepository,
            IStaticCacheManager cacheManager)
        {
            this._contractJointRepository = contractJointRepository;
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
        public virtual IList<ProcuringAgency> GetAllProcuringAgencys(string Name = "")
        {
            var query = _itemRepository.Table;
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name));
            return query
                   .OrderBy(item => item.Name)
                   .ToList();
        }
        public IPagedList<ProcuringAgency> GetProcuringAgencys(int TypeId = 0, string Name = "",bool isInEVN = true, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _itemRepository.Table;
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(c => c.Name.Contains(Name));
            }
            if (isInEVN)
            {
                query = query.Where(c => c.IsInEVN == true);
            }
            else
            {
                query = query.Where(c => c.IsInEVN == false);
            }
            return new PagedList<ProcuringAgency>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual ProcuringAgency GetProcuringAgencyById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }
        public virtual ProcuringAgency GetProcuringAgencyByPECC1()
        {
            return GetProcuringAgencyById(GSDataSettingsDefaults.PECCProcuringAgencyId);
        }
        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertProcuringAgency(ProcuringAgency item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProcuringAgencyByPatternKey);

            //event notification
            _eventPublisher.EntityInserted(item);
        }        
        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateProcuringAgency(ProcuringAgency item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProcuringAgencyByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteProcuringAgency(ProcuringAgency item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.ProcuringAgencyByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }

        public virtual Decimal GetTotalProcuringAgency(int year = 0)
        {
            var query = _itemRepository.Table;
            if (year != 0)
            {
                var queryContract = _contractRepository.Table.Where(c => Convert.ToDateTime(c.StartDate).Year < year)
                    .Select(c => c.Id);
                var queryContractJoin = _contractJointRepository.Table
                    .Where(c => !queryContract.Contains(c.ContractId))
                    .Select(c => c.ProcuringAgencyId);
                query = query.Where(c => queryContractJoin.Contains(c.Id));
            }
            return query.Count();
        }
        public virtual Decimal GetTotalProcuringAgencyIsInEVN(bool isInEVN)
        {
            var query = _itemRepository.Table;
            if (isInEVN == true)
                query = query.Where(c => c.IsInEVN);
            return query.Count();
        }

        #endregion

        #endregion
    }
}
