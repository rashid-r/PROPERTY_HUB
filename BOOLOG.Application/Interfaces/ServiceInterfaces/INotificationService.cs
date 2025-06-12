using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOLOG.Application.Interfaces.ServiceInterfaces
{
    public interface INotificationService
    {
        Task SendToAllAsync(string title, string message);
        Task SendToUserAsync(string userId, string title, string message);
        Task SendToUsersAsync(List<string> userIds, string title, string message);
        Task NotifyPropertySoldAsync(Guid userId, string propertyTitle);
    }
}
