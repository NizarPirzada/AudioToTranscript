using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Utilities.SignalR
{
    public interface IAdminNotificationHub
    {
        Task SendMessageToLoggedUserAsync(string userId, string message, bool success = true);
    }
}
