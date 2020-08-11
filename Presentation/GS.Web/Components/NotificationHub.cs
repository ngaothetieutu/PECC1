using GS.Core.Domain.Messages;
using GS.Core.Infrastructure;
using GS.Services.Messages;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Models.PrivateMessages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Components
{
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.SendAsync("Hello", "Hello World!", "Welcome to PECC1");
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async void Subscribe(string customerId)
        {
            await this.Groups.AddToGroupAsync(Context.ConnectionId, customerId);
            //gui thong bao cho user nay
            var _notificationService = EngineContext.Current.Resolve<INotificationService>();
            // Lay cac notification chua gui di       
            var notifications = _notificationService.GetNotifications(Convert.ToInt32(customerId), new NotificationStatus[] { NotificationStatus.New, NotificationStatus.Sent }, 0);
            if (notifications.Count > 0)
                await this.Clients.Group(customerId).SendAsync("ReceiveMessage", notifications.Select(t => t.ToModel<NotificationModel>()).ToList());
        }
        public async void Unsubscribe(string customerId)
        {
            await this.Groups.RemoveFromGroupAsync(Context.ConnectionId, customerId);
        }
    }
}
