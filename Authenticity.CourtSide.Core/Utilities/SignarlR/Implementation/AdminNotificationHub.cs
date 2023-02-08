using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Utilities.SignalR
{
    [EnableCors("CorsPolicy")]    
    public class AdminNotificationHub : Hub, IAdminNotificationHub
    {
        protected IHubContext<AdminNotificationHub> _hubContext;

        public async Task SendMessageToLoggedUserAsync(string userId, string message, bool success = true)
        {
            await Clients.Caller.SendAsync("NotifyDeletionStep", userId, message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
