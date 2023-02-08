using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Authenticity.CourtSide.Core.Helpers;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class SettingsDomain : ISettingsDomain
	{
		public SettingsDomain(IConfiguration configuration, ISettingsCommandRepository settingsCommandRepository, ISettingsQueryRepository settingsQueryRepository)
		{
			Configuration = configuration;
			SettingsCommandRepository = settingsCommandRepository;
			SettingsQueryRepository = settingsQueryRepository;
		}

		private IConfiguration Configuration { get; }
		private ISettingsCommandRepository SettingsCommandRepository { get; }
		private ISettingsQueryRepository SettingsQueryRepository { get; }

		public async Task<IEnumerable<FileProviderModel>> GetAllFileProvidersAsync()
		{
			IList<FileProviderModel> fileProviders = new List<FileProviderModel>();

			IEnumerable<FileProviderModel> queryResult = await SettingsQueryRepository.GetAllFileProvidersAsync();

			if (queryResult.Any())
			{
				return queryResult;
			}

			var ftpConfiguration = new FtpConfiguration()
			{
				Uri = $"ftp://{Configuration["FTPUri"]}",
				Username = Configuration["FTPUsername"],
				Password = Configuration["FTPPassword"],
				IsSSL = SettingKeyHelper.GetSettingKey<bool>(Configuration, "FTPIsSSL"),
			};

			FileProviderModel fileProvider = new FileProviderModel()
			{
				Name = "FTP/FTPS",
				Parameters = Newtonsoft.Json.JsonConvert.SerializeObject(ftpConfiguration),
				IsCurrentProvider = true
			};

			fileProviders.Add(fileProvider);

			return fileProviders;
		}

		public async Task<FileProviderModel> CreateFileProviderAsync(CreateFileProviderDto createFileProviderDto)
		{
			int fileProviderCreatedId = await SettingsCommandRepository.CreateFileProviderAsync(createFileProviderDto);

			return await SettingsQueryRepository.GetFileProviderByIdAsync(fileProviderCreatedId);
		}

		public async Task<FileProviderModel> UpdateFileProviderAsync(UpdateFileProviderDto updateFileProviderDto)
		{
			await SettingsCommandRepository.UpdateFileProviderAsync(updateFileProviderDto);

            return await SettingsQueryRepository.GetFileProviderByIdAsync(updateFileProviderDto.Id);
        }

        public Task<FileProviderModel> GetCurrentFileProviderAsync()
        {
            return SettingsQueryRepository.GetCurrentFileProviderAsync();
        }
    }
}
