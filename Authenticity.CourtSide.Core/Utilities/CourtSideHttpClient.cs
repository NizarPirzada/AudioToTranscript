using System.Net.Http;

namespace Authenticity.CourtSide.Core.Utilities
{
    public class CourtSideHttpClient : IHttpClientFactory
	{
		public HttpClient CreateClient(string name)
		{
			return new HttpClient();
		}
	}
}
