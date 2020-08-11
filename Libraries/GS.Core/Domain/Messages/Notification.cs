using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Messages
{
    public enum NotificationType
    {
        /// <summary>
        /// Hop dong
        /// </summary>
        Contract=1,
        /// <summary>
        /// Cong viec
        /// </summary>
        Task=2,
        /// <summary>
        /// Khac
        /// </summary>
        Other=3

    }
    public enum NotificationStatus
    {
        All=0,
        /// <summary>
        /// Moi
        /// </summary>
        New=1,
        /// <summary>
        /// Da gui
        /// </summary>
        Sent=2,
        /// <summary>
        /// Da doc
        /// </summary>
        Read=3
    }
    public class Notification : BaseEntity
    {
     
        
        public int ReceiverId { get; set; }
        public virtual Customer receiver { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// Du lieu ID, co the la ID cua object hoac guid cua object
        /// </summary>
        public string DataId { get; set; }
        public int TypeId { get; set; }
        public NotificationType notificationType
        {
            get => (NotificationType)TypeId;
            set => TypeId = (int)value;
        }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public int StatusId { get; set; }
        public NotificationStatus notificationStatus
        {
            get => (NotificationStatus)StatusId;
            set => StatusId = (int)value;
        }
        public int CreatorId { get; set; }
        public virtual Customer creator { get; set; }

        public static Notification CreateSimple(int _creatorId, string _subject, string _body,int _receivedId=0)
        {
            var item =new  Notification();
            item.CreatorId = _creatorId;
            item.Subject = _subject;
            item.Body = _body;
            item.notificationType = NotificationType.Other;
            item.ReceiverId = _receivedId > 0 ? _receivedId: _creatorId;
            
            return item;
        }
    }
}
