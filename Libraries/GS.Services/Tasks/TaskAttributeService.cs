using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.Tasks;
using GS.Services.Events;

namespace GS.Services.Tasks
{
    public partial class TaskAttributeService : ITaskAttributeService
    {
        #region Fields
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<TaskAttribute> _taskAttributeRepository;
        private readonly IRepository<TaskAttributeValue> _taskAttributeValueRepository;
        #endregion

        #region Ctor
        public TaskAttributeService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<TaskAttribute> taskAttributeRepository,
            IRepository<TaskAttributeValue> taskAttributeValueRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._taskAttributeRepository = taskAttributeRepository;
            this._taskAttributeValueRepository = taskAttributeValueRepository;
        }
        #endregion

        #region Methods
        public virtual void DeleteTaskAttribute(TaskAttribute taskAttribute)
        {
            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            _taskAttributeRepository.Delete(taskAttribute);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(taskAttribute);
        }

        public virtual void DeleteTaskAttributeValue(TaskAttributeValue taskAttributeValue)
        {
            if (taskAttributeValue == null)
                throw new ArgumentNullException(nameof(taskAttributeValue));

            _taskAttributeValueRepository.Delete(taskAttributeValue);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(taskAttributeValue);
        }

        public virtual IList<TaskAttribute> GetAllTaskAttributes()
        {
            return _cacheManager.Get(GSTaskServiceDefaults.TaskAttributesAllCacheKey, () =>
            {
                var query = from ca in _taskAttributeRepository.Table
                            orderby ca.DisplayOrder, ca.Id
                            select ca;
                return query.ToList();
            });
        }

        public virtual TaskAttribute GetTaskAttributeById(int taskAttributeId)
        {
            if (taskAttributeId == 0)
                return null;

            var key = string.Format(GSTaskServiceDefaults.TaskAttributesByIdCacheKey, taskAttributeId);
            return _cacheManager.Get(key, () => _taskAttributeRepository.GetById(taskAttributeId));
        }

        public virtual TaskAttributeValue GetTaskAttributeValueById(int taskAttributeValueId)
        {
            if (taskAttributeValueId == 0)
                return null;

            var key = string.Format(GSTaskServiceDefaults.TaskAttributeValuesByIdCacheKey, taskAttributeValueId);
            return _cacheManager.Get(key, () => _taskAttributeValueRepository.GetById(taskAttributeValueId));
        }

        public virtual IList<TaskAttributeValue> GetTaskAttributeValues(int taskAttributeId)
        {
            var key = string.Format(GSTaskServiceDefaults.TaskAttributeValuesAllCacheKey, taskAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from cav in _taskAttributeValueRepository.Table
                            orderby cav.DisplayOrder, cav.Id
                            where cav.TaskAttributeId == taskAttributeId
                            select cav;
                var taskAttributeValues = query.ToList();
                return taskAttributeValues;
            });
        }

        public virtual void InsertTaskAttribute(TaskAttribute taskAttribute)
        {
            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            _taskAttributeRepository.Insert(taskAttribute);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(taskAttribute);
        }

        public virtual void InsertTaskAttributeValue(TaskAttributeValue taskAttributeValue)
        {
            if (taskAttributeValue == null)
                throw new ArgumentNullException(nameof(taskAttributeValue));

            _taskAttributeValueRepository.Insert(taskAttributeValue);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(taskAttributeValue);
        }

        public virtual void UpdateTaskAttribute(TaskAttribute taskAttribute)
        {
            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            _taskAttributeRepository.Update(taskAttribute);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(taskAttribute);
        }

        public virtual void UpdateTaskAttributeValue(TaskAttributeValue taskAttributeValue)
        {
            if (taskAttributeValue == null)
                throw new ArgumentNullException(nameof(taskAttributeValue));

            _taskAttributeValueRepository.Update(taskAttributeValue);

            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(GSTaskServiceDefaults.TaskAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(taskAttributeValue);
        }
        #endregion
    }
}
