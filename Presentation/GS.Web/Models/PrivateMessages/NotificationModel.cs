using GS.Core;
using GS.Core.Domain.Messages;
using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.PrivateMessages
{
    public class NotificationModel : BaseGSEntityModel
    {
        public int ReceiverId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// Du lieu ID, co the la ID cua object hoac guid cua object
        /// </summary>
        public string DataId { get; set; }
        public int TypeId { get; set; }
        public string TypeIcon
        {
            get
            {
                switch (notificationType)
                {
                    case NotificationType.Other:
                        return "ion-md-megaphone";
                    case NotificationType.Contract:
                        return "ion-ios-albums";
                    case NotificationType.Task:
                        return "ion-md-alarm";
                }
                return "ion-md-megaphone";

            }
        }
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
        public string CreatorName { get; set; }
        public string DistanceTime => CreatedDate.ToDistanceTime();
    }
    public partial class NotificationSearchModel : BaseSearchModel
    {
        public int CustomerId { get; set; }
    }
    public partial class NotificationListModel : BasePagedListModel<NotificationModel>
    {

    }
}
