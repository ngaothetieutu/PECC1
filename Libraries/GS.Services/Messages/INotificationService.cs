using System;
using GS.Core.Domain.Messages;
using System.Collections.Generic;
using System.Text;
using GS.Services.Events;
using GS.Core.Data;

namespace GS.Services.Messages
{
    public partial interface INotificationService
    {
        
        #region Notification

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Notification</param>
        void DeleteNotification(Notification item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        List<Notification> GetAllNotifications();

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Notification</param>
        /// <returns>Notification</returns>
        List<Notification> GetNotifications(int CustomerId,NotificationStatus[] StatusIds=null,int LastViewId=0, int Top=5  );

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Notification</param>
        void InsertNotification(Notification item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Notification</param>
        void UpdateNotification(Notification item);
        void UpdateStatusNotification(List<Notification> items, NotificationStatus StatusId);
        void UpdateStatusNotificationToRead(int Id);
        List<Notification> GetMoreNotification(int CustomerId, int i, int s);
        #endregion
    }
}
