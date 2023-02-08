using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Models.Transcription;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Resources;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.DataProviders.Implementation
{
	public class AuthenticityAuthenticationDataProvider : IAuthenticationDataProvider
	{
		private const string REQUEST_SUCCESS_STATUS = "success";
		private const string DEFAULT_TRANSCRIPTION_LANGUAGE = "en";
		private const string ERROR_OBTAINING_USER_GUID = "The User GUID API cannot be obtained, please contact the administrator";
		private const string ERROR_INVALID_USER_GUID = "The API GUID is not valid to establish a connection";
		private const string ERROR_INVALID_USER_URL = "The API URL is not accessible";

		public IHttpClientFactory HttpClient { get; }
		public LocalFileHelper LocalFileHelper { get; }

		private string ApiEndpoint { get; }

		public AuthenticityAuthenticationDataProvider(IConfiguration configuration, IHttpClientFactory httpClient, LocalFileHelper localFileHelper)
		{
			HttpClient = httpClient;
			LocalFileHelper = localFileHelper;
			ApiEndpoint = configuration["TranscriptionApiEndpoint"];
		}
		public async Task<string> GetUserGuid(string serverUrl)
		{
			try
			{
				HttpClient httpClient = HttpClient.CreateClient();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

				var formDataContent = new MultipartFormDataContent();
				formDataContent.Add(new StringContent("DB408BEA"), "userid");

				HttpResponseMessage response = await httpClient.PostAsync($"{serverUrl}vr_api/users/add_user", formDataContent);
				string parsedResponse = await response.Content.ReadAsStringAsync();
				AuthenticityUserRegisterModel userRegister = JsonConvert.DeserializeObject<AuthenticityUserRegisterModel>(parsedResponse);

				return userRegister.Status.Equals(REQUEST_SUCCESS_STATUS) ? userRegister?.Guid : string.Empty;
			}
			catch
			{
				throw new ApplicationException(ERROR_OBTAINING_USER_GUID);
			}
		}

		public async Task<bool> CheckUserAPICredentialsAsync(UserApiCredentials userApiCredentials)
		{
			FileStream file = null;
			try
			{
				string filePath =
					await LocalFileHelper.SaveUnmanagedMemoryStreamAsync(AudioFileResources.zero_wav,
						nameof(AudioFileResources.zero_wav).Replace("_", "."));
				
				file = File.Open(filePath, FileMode.OpenOrCreate);

				HttpClient httpClient = HttpClient.CreateClient();
				httpClient.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("multipart/form-data"));

				TranscriptEngineParameter engineParameters = new AuthenticityTranscriptEngineRequest()
				{
					file = file,
					engine = userApiCredentials.ApiEngine,
					language = DEFAULT_TRANSCRIPTION_LANGUAGE,
					guid = userApiCredentials.ApiGuid,
				};

				MultipartFormDataContent formData = engineParameters.MapMultipart();
				HttpResponseMessage response = await httpClient.PostAsync($"{userApiCredentials.ApiUrl}{ApiEndpoint}", formData);
				return response.IsSuccessStatusCode ? true : throw new Exception();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("No such host is known"))
				{
					throw new ApplicationException(ERROR_INVALID_USER_URL);
				}

				throw new ApplicationException(ERROR_INVALID_USER_GUID);
			}
			finally
			{
				file.Close();
			}
		}
	}
}
