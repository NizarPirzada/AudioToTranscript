using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.Domains.Context
{
	public interface IUserContext
	{
		IUserCommandRepository UserCommandRepository { get; }
		IUserQueryRepository UserQueryRepository { get; }
		IRoleQueryRepository RoleQueryRepository { get; }
		IEmailDomain EmailDomain { get; }
		IPasswordHasher PasswordHasher { get; }
		IUserLoginHistoryQueryRepository UserLoginHistoryQueryRepository { get; }
		IAuthenticationDataProvider AuthenticationDataProvider { get; }
		IConfiguration Configuration { get; }
		ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
		IFileProviderFactory FileProviderFactory { get; }
		ITranscriptQueryRepository TranscriptQueryRepository { get; }
		ITranscriptDialogCommandRepository TranscriptDialogCommandRepository { get; }
		ITranscriptPersonCommandRepository TranscriptPersonCommandRepository { get; }
		ICourtCommandRepository CourtCommandRepository { get; }
		ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
		ITranscriptRecordingInfoCommandRepository TranscriptRecordingInfoCommandRepository { get; }
		ITranscriptCommandRepository TranscriptCommandRepository { get; }
		ITranscriptionEngineQueryRepository TranscriptionEngineQueryRepository { get; }
	}
}
