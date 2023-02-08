using Authenticity.CourtSide.Core.DataProviders.Context;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation
{
	public class FileProviderFactory : IFileProviderFactory
	{
		public IFileProviderFactoryContext FileProviderFactoryContext { get; }

		public FileProviderFactory(IFileProviderFactoryContext fileProviderFactoryContext)
		{
			FileProviderFactoryContext = fileProviderFactoryContext;
		}

		public async Task<IFileProvider> CreateAsync()
		{
			FileProviderModel currentFileProvider = await GetCurrentFileProviderAsync();

			string key = currentFileProvider?.Name;

			switch (key)
			{
				case "FTP/FTPS":
					FtpConfiguration ftpParamters = JsonConvert.DeserializeObject<FtpConfiguration>(currentFileProvider.Parameters);
					return new FTPProvider(ftpParamters, FileProviderFactoryContext.FtpProviderLogger);
				case "SFTP":
					SftpConfiguration sftpParamters = JsonConvert.DeserializeObject<SftpConfiguration>(currentFileProvider.Parameters);
					return new SFTPProvider(sftpParamters, FileProviderFactoryContext.SftpProviderLogger);
				default:
					throw new KeyNotFoundException();
			}
		}

		private async Task<FileProviderModel> GetCurrentFileProviderAsync()
		{
			FileProviderModel fileProviderModel = await FileProviderFactoryContext.SettingsQueryRepository.GetCurrentFileProviderAsync();

			if (fileProviderModel == null)
			{
				var ftpConfiguration = new FtpConfiguration()
				{
					Uri = $"ftp://{FileProviderFactoryContext.Configuration["FTPUri"]}",
					Username = FileProviderFactoryContext.Configuration["FTPUsername"],
					Password = FileProviderFactoryContext.Configuration["FTPPassword"],
					IsSSL = SettingKeyHelper.GetSettingKey<bool>(FileProviderFactoryContext.Configuration, "FTPIsSSL"),
				};

				var createFileProviderDto = new CreateFileProviderDto()
				{
					Name = "FTP/FTPS",
					Parameters = JsonConvert.SerializeObject(ftpConfiguration),
					IsCurrentProvider = true
				};

				int fileProviderCreationResult = await FileProviderFactoryContext.SettingsCommandRepository.CreateFileProviderAsync(createFileProviderDto);

				fileProviderModel = await FileProviderFactoryContext.SettingsQueryRepository.GetFileProviderByIdAsync(fileProviderCreationResult);
			}

			return fileProviderModel;
		}

	}
}
