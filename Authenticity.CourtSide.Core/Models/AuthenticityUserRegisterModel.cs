using System.Linq;
using Newtonsoft.Json;

namespace Authenticity.CourtSide.Core.Models
{
	public class AuthenticityUserRegisterModel
	{

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		public string Guid => Message.Split(' ').LastOrDefault();
	}
}
