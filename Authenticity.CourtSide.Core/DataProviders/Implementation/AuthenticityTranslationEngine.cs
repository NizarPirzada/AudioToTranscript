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
	public class AuthenticityTranslationEngine : ITranslationEngine
	{
		private const int EXPORT_TIMEOUT_IN_MINUTES = 60;
		private const string DEFAULT_TRANSCRIPTION_ENGINE = "xcel-0";
		private const string DEFAULT_TRANSCRIPTION_LANGUAGE = "en";
		private const string DEFAULT_TRANSLATION_MODE = "normal";
		private const string ERROR_INVALID_USER_URL = "The API URL is not accessible";

		private readonly IConfiguration _configuration;
		private IHttpClientFactory _httpClient;
		private string _apiEndpoint { get; set; }

		public AuthenticityTranslationEngine(IConfiguration configuration, IHttpClientFactory httpClient)
		{
			_configuration = configuration;
			_httpClient = httpClient;
			_apiEndpoint = _configuration["TranslationApiEndpoint"];
		}

		public async Task<string> CreateTranslationRequestAsync(TranslationRequestModel translationRequest)
		{
			FileStream file = File.Open(translationRequest.PathFile, FileMode.Open);

			HttpClient httpClient = _httpClient.CreateClient();

			httpClient.Timeout = TimeSpan.FromMinutes(EXPORT_TIMEOUT_IN_MINUTES);
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

			TranslationEngineParameter engineParameters = new AuthenticityTranslationEngineRequest()
			{
				file = file,
				from_lang = translationRequest.SourceLanguage,
				to_lang = translationRequest.OutputLanguage,
				guid = translationRequest.ApiGuid,
				mode = DEFAULT_TRANSLATION_MODE
			};

			MultipartFormDataContent formData = engineParameters.MapMultipart();

			HttpResponseMessage response = await httpClient.PostAsync($"{translationRequest.ApiUrl}{_apiEndpoint}", formData);
			string parsedResponse = await response.Content.ReadAsStringAsync();
			IEnumerable<AuthenticityTranslationEngineResponse> clientCreatedResponse = JsonConvert.DeserializeObject<IEnumerable<AuthenticityTranslationEngineResponse>>(parsedResponse);
			file.Close();

			return clientCreatedResponse.FirstOrDefault()?.TaskId;
		}

		public async Task<Stream> CheckTranslationResponseAsync(string serverUrl, string taskId)
		{
			try
			{

				HttpClient httpClient = _httpClient.CreateClient();

				httpClient.Timeout = TimeSpan.FromMinutes(EXPORT_TIMEOUT_IN_MINUTES);

				HttpResponseMessage response = await httpClient.GetAsync($"{serverUrl}{_apiEndpoint}/{taskId}");
				string stringParsedResponse = await response.Content.ReadAsStringAsync();
				if (stringParsedResponse.Contains("\"status\": \"PENDING\""))
				{
					return null;
				}
				else
				{
					Stream parsedResponse = await response.Content.ReadAsStreamAsync();
					return parsedResponse;
				}
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
