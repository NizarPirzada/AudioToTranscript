using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders
{
	public interface IAuthenticationDataProvider
	{
		Task<string> GetUserGuid(string serverUrl);
		Task<bool> CheckUserAPICredentialsAsync(UserApiCredentials userApiCredentials);
	}
}
