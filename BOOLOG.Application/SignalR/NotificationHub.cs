using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BOOLOG.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessageToAll(string title, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", title, message);
        }

        public async Task SendMessageToCaller(string title, string message)
        {
            await Clients.Caller.SendAsync("ReceiveNotification", title, message); // Action performing Current user only
        }

        public async Task SendMessageToUser(string userId, string title, string message) // Specific user
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        }

        public async Task SendMessageToOthers(string title, string message) // All users Except current user Eg: Special Offer for a property
        {
            await Clients.Others.SendAsync("ReceiveNotification", title, message);
        }

        //public async Task SendMessageToConnection(string connectionId, string title, string message) // Notify when using other device (Clients.client)
        //{
        //    await Clients.Client(connectionId).SendAsync("ReceiveNotification", title, message);
        //}
    }
}
