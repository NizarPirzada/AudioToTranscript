using Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authenticity.CourtSide.Core.DataProviders.Context.Implementation
{
	public class FileProviderFactoryContext : IFileProviderFactoryContext
	{
		public IConfiguration Configuration { get; }
		public ISettingsQueryRepository SettingsQueryRepository { get; }
		public ISettingsCommandRepository SettingsCommandRepository { get; }
		public ILogger<FTPProvider> FtpProviderLogger { get; }
		public ILogger<SFTPProvider> SftpProviderLogger { get; }

		public FileProviderFactoryContext(IConfiguration configuration, ISettingsQueryRepository settingsQueryRepository, ISettingsCommandRepository settingsCommandRepository, ILogger<FTPProvider> ftpProviderLogger, ILogger<SFTPProvider> sftpProviderLogger)
		{
			SettingsQueryRepository = settingsQueryRepository;
			SettingsCommandRepository = settingsCommandRepository;
			FtpProviderLogger = ftpProviderLogger;
			SftpProviderLogger = sftpProviderLogger;
			Configuration = configuration;
		}
	}
}
