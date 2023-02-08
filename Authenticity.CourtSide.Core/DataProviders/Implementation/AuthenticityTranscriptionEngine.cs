using Authenticity.CourtSide.Core.Models.Transcription;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders.Implementation
{
	public class AuthenticityTranscriptionEngine : ITranscriptionEngine
	{
		private const int EXPORT_TIMEOUT_IN_MINUTES = 60;
		private const string ERROR_INVALID_USER_URL = "The API URL is not accessible";

		private readonly IConfiguration _configuration;
		private IHttpClientFactory _httpClient;
		private string _apiEndpoint { get; set; }

		public AuthenticityTranscriptionEngine(IConfiguration configuration, IHttpClientFactory httpClient)
		{
			_configuration = configuration;
			_httpClient = httpClient;
			_apiEndpoint = _configuration["TranscriptionApiEndpoint"];
		}

		public async Task<string> CreateTranscriptRequestAsync(TranscriptRequestModel transcriptRequest)
		{
			FileStream file = File.Open(transcriptRequest.PathFile, FileMode.Open);

			HttpClient httpClient = _httpClient.CreateClient();

			httpClient.Timeout = TimeSpan.FromMinutes(EXPORT_TIMEOUT_IN_MINUTES);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

			TranscriptEngineParameter engineParameters = new AuthenticityTranscriptEngineRequest()
			{
				file = file,
				engine = transcriptRequest.TranscriptionEngine,
				language = transcriptRequest.ApiLanguage,
				guid = transcriptRequest.ApiGuid
			};

			MultipartFormDataContent formData = engineParameters.MapMultipart();

			HttpResponseMessage response = await httpClient.PostAsync($"{transcriptRequest.ApiUrl}{_apiEndpoint}", formData);
			string parsedResponse = await response.Content.ReadAsStringAsync();
			IEnumerable<AuthenticityTranscriptEngineResponse> clientCreatedResponse = JsonConvert.DeserializeObject<IEnumerable<AuthenticityTranscriptEngineResponse>>(parsedResponse);
			file.Close();

			return clientCreatedResponse.FirstOrDefault()?.TaskId;
		}

		public async Task<string> CheckTranscriptResponseAsync(string serverUrl, string taskId)
		{
			try
			{

				HttpClient httpClient = _httpClient.CreateClient();

				httpClient.Timeout = TimeSpan.FromMinutes(EXPORT_TIMEOUT_IN_MINUTES);

				HttpResponseMessage response = await httpClient.GetAsync($"{serverUrl}{_apiEndpoint}/{taskId}");
				string parsedResponse = await response.Content.ReadAsStringAsync();

				return parsedResponse;
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("No such host is known") || ex.Message.Contains("An invalid request URI was provided"))
				{
					throw new ApplicationException(ERROR_INVALID_USER_URL);
				}

				throw ex;
			}
		}
	}
}
