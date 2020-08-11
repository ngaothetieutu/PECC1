using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Caching;
using GS.Core.Data;
using GS.Core.Domain.WidgetApps;
using GS.Services.Events;
using System.Linq;

namespace GS.Services.WidgetApps
{
    public class WidgetAppService : IWidgetAppService
    {
        #region Fields
        private readonly IRepository<WidgetApp> _itemRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public WidgetAppService(IRepository<WidgetApp> itemRepository,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._itemRepository = itemRepository;
            this._eventPublisher = eventPublisher;
        }
        #endregion

        #region Methods
        public virtual void DeleteWidgetApp(WidgetApp item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }

        public virtual IList<WidgetApp> GetAllWidgetApps()
        {
            return _cacheManager.Get(GSWidgetAppServiceDefaults.WidgetAppsAllCacheKey, () =>
            {
                return _itemRepository.Table
                .OrderBy(c => c.Name)
                .ToList();
            });
        }

        public virtual WidgetApp GetWidgetAppById(int widgetAppId)
        {
            if (widgetAppId == 0)
                return null;

            var key = string.Format(GSWidgetAppServiceDefaults.WidgetAppsByIdCacheKey, widgetAppId);
            return _cacheManager.Get(key, () => _itemRepository.GetById(widgetAppId));
        }

        public virtual void InsertWidgetApp(WidgetApp item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public virtual void UpdateWidgetApp(WidgetApp item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _itemRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
    }
}
