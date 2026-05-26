using Microsoft.AspNetCore.SignalR;
using UrbanHub.Entities;

namespace UrbanHub.web.custom_services
{
    public class SignalrNotification : Hub
    {
        public async Task SendNotifications(int uid, string message)
        {
            //await Clients.User(uid.ToString()).SendAsync("ReceiveNotification", message);
            await Clients.All.SendAsync("ReceiveMessage", uid, message);
        }
    }
}
