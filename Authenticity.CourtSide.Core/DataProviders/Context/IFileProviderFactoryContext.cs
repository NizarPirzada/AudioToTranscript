using Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authenticity.CourtSide.Core.DataProviders.Context
{
	public interface IFileProviderFactoryContext
	{
		IConfiguration Configuration { get; }
		ISettingsQueryRepository SettingsQueryRepository { get; }
		ISettingsCommandRepository SettingsCommandRepository { get; }
		ILogger<FTPProvider> FtpProviderLogger { get; }
		ILogger<SFTPProvider> SftpProviderLogger { get; }
	}
}
