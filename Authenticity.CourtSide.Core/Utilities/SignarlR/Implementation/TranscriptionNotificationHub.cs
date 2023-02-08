using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Utilities.SignarlR.Implementation
{
	[EnableCors("CorsPolicy")]
	public class TranscriptionNotificationHub : Hub, ITranscriptionNotificationHub
	{
		protected IHubContext<TranscriptionNotificationHub> _hubContext;
		private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
		public TranscriptionNotificationHub(IHubContext<TranscriptionNotificationHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyExaminationSavingStatusAsync(string userId, string message, bool success = true)
		{
			IEnumerable<string> userConnections = _connections.GetConnections(userId);

			foreach (var connectionId in userConnections)
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("NotifyExaminationSavingStatus", userId, message, success);
			}
		}

		public string GetConnectionId()
		{
			return Context.ConnectionId;
		}

		public override Task OnConnectedAsync()
		{
			string userToken = GetUserToken();
			_connections.Add(userToken, Context.ConnectionId);

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(System.Exception exception)
		{
			string userToken = GetUserToken();
			_connections.Remove(userToken, Context.ConnectionId);

			return base.OnDisconnectedAsync(exception);
		}

		private string GetUserToken()
		{
			return Context.GetHttpContext().Request.Query["userToken"];
		}

	}
}
