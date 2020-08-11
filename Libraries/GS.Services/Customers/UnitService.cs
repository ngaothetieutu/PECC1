using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Customers;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GS.Services.Customers
{
    public class UnitService:IUnitService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CustomerUnitMapping> _itemCustomerUnitRepository;
        private readonly IRepository<Unit> _itemUnitRepository;
        
        #endregion

        #region Ctor

        public UnitService(IEventPublisher eventPublisher,
            IRepository<Unit> itemRepository,
            IRepository<CustomerUnitMapping> itemCustomerUnitRepository)
        {
            this._eventPublisher = eventPublisher;
            this._itemUnitRepository = itemRepository;
            this._itemCustomerUnitRepository = itemCustomerUnitRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all review types
        /// </summary>
        /// <returns>Review types</returns>
        public virtual IList<Unit> GetAllUnits(string Name = null, int? ParentId = null)
        {
            var query = _itemUnitRepository.Table;
            if (ParentId != null)
            {
                query = query.Where(c => c.ParentId != null && c.ParentId == ParentId);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(c => c.Name.Contains(Name));
            }            
            return query
                    .OrderBy(item => item.TreeNode)
                    .ToList();
        }
        
        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual Unit GetUnitById(int itemId)
        {
            if (itemId == 0)
                return null;
            var lst= _itemUnitRepository.GetById(itemId);
            return lst;
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual bool InsertUnit(Unit item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var param = _itemUnitRepository.Table.Where(c => c.Code == item.Code).Count();
            if (param > 0)
                return false;
            if (item.StoreId == null)
                item.StoreId = 1;
            if (item.ParentId == 0)
                item.ParentId = null;
            if (string.IsNullOrEmpty(item.TreeNode))
                item.TreeNode = "";
            if (item.TreeLevel == null)
                item.TreeLevel = 0;

            _itemUnitRepository.Insert(item);
            if (item.ParentId == null)
            {
                item.TreeNode = item.Id.ToString().PadLeft(5, '0');
                item.TreeLevel = 1;
                item.LevelSpace = "-";
            }
            else
            {
                var query = _itemUnitRepository.Table.Where(c=>c.Id==item.ParentId).ToList();
                item.TreeNode = query[0].TreeNode + "-" + item.Id.ToString().PadLeft(5, '0');
                item.TreeLevel = query[0].TreeLevel + 1;
                item.LevelSpace = query[0].LevelSpace + "-";
            }
            _itemUnitRepository.Update(item);
            //event notification
            _eventPublisher.EntityInserted(item);
            return true;
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual bool UpdateUnit(Unit item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var param = _itemUnitRepository.Table.Where(c => c.Id != item.Id && c.Code == item.Code).Count();
            if (param > 0)
                return false;
            if (item.ParentId == 0 || item.ParentId == item.Id)
                item.ParentId = null;
            if (item.ParentId == null)
            {
                item.TreeNode = item.Id.ToString().PadLeft(5, '0');
                item.TreeLevel = 1;
                item.LevelSpace = "-";
            }
            else
            {
                var query = _itemUnitRepository.Table.Where(c => c.Id == item.ParentId).ToList();
                item.TreeNode = query[0].TreeNode + "-" + item.Id.ToString().PadLeft(5, '0');
                item.TreeLevel = query[0].TreeLevel + 1;
                item.LevelSpace = query[0].LevelSpace + "-";
            }
            _itemUnitRepository.Update(item);

            //event notification
            _eventPublisher.EntityUpdated(item);
            return true;
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual bool DeleteUnit(Unit item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                _itemUnitRepository.Delete(item);

                //event notification
                _eventPublisher.EntityDeleted(item);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public virtual IList<Customer> GetAllCustomerByUnitId(int UnitId)
        {
            if (UnitId == 0)
                return null;
            return _itemCustomerUnitRepository.Table.Where(c => c.UnitId == UnitId)
                    .Select(item => item.Customer).ToList();
                    
        }
        public bool insertCustomerUnit(int UnitId, int CustomerId)
        {
            try
            {
                CustomerUnitMapping item = new CustomerUnitMapping();
                item.UnitId = UnitId;
                item.CustomerId = CustomerId;
                _itemCustomerUnitRepository.Insert(item);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool deleteCustomerUnit(int UnitId, int CustomerId)
        {
            try
            {
                var items = _itemCustomerUnitRepository.Table;
                items = items.Where(c => c.UnitId == UnitId && c.CustomerId == CustomerId);
                _itemCustomerUnitRepository.Delete(items);
                return true;
            }
            catch
            {
                return false;
            } 
        }

        public virtual Decimal GetTotalUnit()
        {
            var query = _itemUnitRepository.Table;
            return query.Count();
        }

        #endregion
    }
}
