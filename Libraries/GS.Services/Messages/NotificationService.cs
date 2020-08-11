using GS.Core.Data;
using GS.Core.Domain.Messages;
using GS.Data;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GS.Services.Messages
{
    public class NotificationService : INotificationService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Notification> _itemRepository;
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        public NotificationService(
           IRepository<Notification> itemRepository,
            IEventPublisher eventPublisher,
            IDbContext dbContext,
            IDataProvider dataProvider
            )
        {
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._dataProvider = dataProvider;
        }
        public void DeleteNotification(Notification item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Delete(item);
        }

        public List<Notification> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public List<Notification> GetNotifications(int CustomerId, NotificationStatus[] StatusIds = null, int LastViewId = 0, int Top = 5)
        {
            var query = _itemRepository.Table;
            if (CustomerId > 0)
                query = query.Where(c => c.ReceiverId == CustomerId);
            if(StatusIds!=null)
            {
                var _statusIds = StatusIds.Select(c=>(int)c).ToArray();
                query = query.Where(c => _statusIds.Contains(c.StatusId));
            }
            if (LastViewId > 0)
                query = query.Where(c => c.Id < LastViewId);
            query = query.OrderByDescending(c => c.Id);
            if (Top > 0)
                return query.Take(Top).ToList();
            return query.ToList();
        }
        public List<Notification> GetMoreNotification(int CustomerId, int i, int s)
        {
            var query = _itemRepository.Table;
            if (CustomerId > 0)
                query = query.Where(c => c.ReceiverId == CustomerId);
            query = query.OrderByDescending(c => c.Id);
            query = query.Skip(s)
                .Take(i);
            return query.ToList();
        }

        public void InsertNotification(Notification item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
            item.notificationStatus = NotificationStatus.New;
            _itemRepository.Insert(item);


        }

        public void UpdateNotification(Notification item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _itemRepository.Update(item);
        }
        public void UpdateStatusNotification(List<Notification> items, NotificationStatus StatusId)
        {
            foreach(var item in items)
            {
                if (StatusId == NotificationStatus.Sent)
                    item.SentDate = DateTime.Now;
                if (StatusId == NotificationStatus.Read)
                    item.ReadDate = DateTime.Now;
                item.notificationStatus = StatusId;
            }
            _itemRepository.Update(items);
        }

        public void UpdateStatusNotificationToRead(int Id)
        {
            var notiId = _dataProvider.GetParameter();
            notiId.ParameterName = "Id";
            notiId.Value = Id;
            notiId.DbType = DbType.Int32;
            _dbContext.ExecuteSqlCommand("EXEC [ReadNotification] @Id",
               false, 600, notiId);
        }
    }
}
