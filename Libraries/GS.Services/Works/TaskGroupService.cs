using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Works;
using GS.Services.Catalog;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GS.Core;

namespace GS.Services.Works
{
    public class TaskGroupService:ITaskGroupService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<TaskGroup> _itemRepository;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public TaskGroupService(IEventPublisher eventPublisher,
            IRepository<TaskGroup> itemRepository,
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
        public virtual IList<TaskGroup> GetAllTaskGroups()
        {
            return _cacheManager.Get(GSCatalogDefaults.TaskGroupAllKey, () =>
            {
                return _itemRepository.Table
                    .OrderBy(item => item.TreeNode)
                    .ToList();
            });
        }

        public virtual IList<TaskGroup> GetAllTaskGroupsByName(string Name = "", int? ParentId = 0)
        {
            var query = _itemRepository.Table;
            if (ParentId == 0 || ParentId == null)
            {
                query = query.Where(c => c.ParentId == null);
            }
            if (ParentId > 0)
            {
                query = query.Where(c => c.ParentId == ParentId);

            }
            if (!string.IsNullOrEmpty(Name))
                query = query.Where(c => c.Name.Contains(Name));
            return query
                   .OrderBy(item => item.TreeNode)
                   .ToList();
        }
        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual TaskGroup GetTaskGroupById(int itemId)
        {
            if (itemId == 0)
                return null;

            var key = string.Format(GSCatalogDefaults.TaskGroupByIdKey, itemId);
            return _cacheManager.Get(key, () => _itemRepository.GetById(itemId));
        }

        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertTaskGroup(TaskGroup item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            //item.TreeLevel = 0;
            if (item.ParentId ==0)
            {
                item.ParentId = null;
            }
           
            _itemRepository.Insert(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.TaskGroupByPatternKey);
            item.TreeNode = item.Id.ToString();
            _itemRepository.Update(item);
            UpdateTaskGroupComplate(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateTaskGroup(TaskGroup item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.ParentId == 0)
                item.ParentId = null;
            _itemRepository.Update(item);
            UpdateTaskGroupComplate(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.TaskGroupByPatternKey);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteTaskGroup(TaskGroup item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(GSCatalogDefaults.TaskGroupByPatternKey);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }
        public void UpdateTaskGroupComplate(TaskGroup item)
        {
            var code = item.Id.toCode(7);
            //item.Code = code;
            item.TreeNode = code;
            //update Code
            if (item.ParentId > 0)  
            {
                //lay thong tin ma cha
                var taskcha = GetTaskGroupById((int)item.ParentId);
                item.TreeNode = taskcha.TreeNode + "-" + code;
                item.TreeLevel = taskcha.TreeLevel + 1;
            }
            _itemRepository.Update(item);
        }

        public int TaskGroupChildCount(int Id)
        {
            return _itemRepository.Table.Where(c => c.ParentId == Id).Count();
        }

        #endregion

        #endregion
    }
}
