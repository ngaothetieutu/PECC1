using GS.Core.Data;
using GS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Threading;
using GS.Services.Messages;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Models.PrivateMessages;
using GS.Core.Domain.Messages;

namespace GS.Web.Components
{

    public class NotificationComponent
    {
        private static string connectionString { get; set; }
        private SqlDependency dependency = null;

        public NotificationComponent(
            )
        {
        }
        public virtual void RegisterNotification()
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = DataSettingsManager.LoadSettings().DataConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (var command = new SqlCommand("SELECT Id FROM [dbo].[Notification]", connection))
                {
                    command.Notification = null;
                    if (dependency == null)
                    {
                        dependency = new SqlDependency(command);
                        dependency.OnChange += new OnChangeEventHandler(Dependency_OnChange);
                    }
                    //do nothing
                    command.ExecuteReader();
                }
            }
        }
        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            // Refresh event change db
            if (dependency != null)
            {
                dependency.OnChange -= Dependency_OnChange;
                dependency = null;
            }
            //try
            //{

            //}
            //catch
            //{
            //}
            //finally
            //{

            //}
            if (e.Type == SqlNotificationType.Change)
            {
                // Cho cac thong bao duoc insert vao bang
                //Thread.Sleep(2000);
                PushNotification();
                //Console.WriteLine("Change DB");
            }
            //Thread.Sleep(1000);
            // call back event change db
            RegisterNotification();
        }

        private void PushNotification()
        {
            var _notificationService = EngineContext.Current.Resolve<INotificationService>();
            // Lay cac notification chua gui di       

            var notifications = _notificationService.GetNotifications(0, new NotificationStatus[] { NotificationStatus.New }, 0, 0);
            var _hubcontext = EngineContext.Current.Resolve<IHubContext<NotificationHub>>();
            //lay tat ca client dang nhan dc de gui di
            var receiveIds = notifications.Select(c => c.ReceiverId).ToList();
            foreach (var recId in receiveIds)
            {
                var items = notifications.Where(c => c.ReceiverId == recId).Select(t => t.ToModel<NotificationModel>()).ToList();
                _hubcontext.Clients.Group(recId.ToString()).SendAsync("ReceiveMessage", items);
                //Thread.Sleep(100);
            }
            //Update da gui notification
            _notificationService.UpdateStatusNotification(notifications, Core.Domain.Messages.NotificationStatus.Sent);
        }

    }
}
