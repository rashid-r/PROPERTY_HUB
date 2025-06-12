using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using BOOLOG.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BOOLOG.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToAllAsync(string title, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", title, message);
        }

        public async Task SendToUserAsync(string userId, string title, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", title, message);
        }

        public async Task SendToUsersAsync(List<string> userIds, string title, string message)
        {
            foreach (var userId in userIds)
            {
                await SendToUserAsync(userId, title, message);
            }
        }

        public async Task NotifyPropertySoldAsync(Guid userId, string propertyTitle)
        {
            string message = $"Your property '{propertyTitle}' has been marked as sold. Congratulations!";
            await SendToUserAsync(userId.ToString(), "Property Sold", message);
        }
    }
}
