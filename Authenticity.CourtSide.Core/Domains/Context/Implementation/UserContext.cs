using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.Domains.Context.Implementation
{
	public class UserContext : IUserContext
	{
		public IUserCommandRepository UserCommandRepository { get; }
		public IUserQueryRepository UserQueryRepository { get; }
		public IRoleQueryRepository RoleQueryRepository { get; }
		public IEmailDomain EmailDomain { get; }
		public IPasswordHasher PasswordHasher { get; }
		public IUserLoginHistoryQueryRepository UserLoginHistoryQueryRepository { get; }
		public IAuthenticationDataProvider AuthenticationDataProvider { get; }
		public IConfiguration Configuration { get; }
		public ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
		public IFileProviderFactory FileProviderFactory { get; }
		public ITranscriptQueryRepository TranscriptQueryRepository { get; }
		public ITranscriptDialogCommandRepository TranscriptDialogCommandRepository { get; }
		public ITranscriptPersonCommandRepository TranscriptPersonCommandRepository { get; }
		public ICourtCommandRepository CourtCommandRepository { get; }
		public ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
		public ITranscriptRecordingInfoCommandRepository TranscriptRecordingInfoCommandRepository { get; }
		public ITranscriptCommandRepository TranscriptCommandRepository { get; }
		public ITranscriptionEngineQueryRepository TranscriptionEngineQueryRepository { get; }

		public UserContext(
			IUserCommandRepository userCommandRepository,
			IUserQueryRepository userQueryRepository,
			IRoleQueryRepository roleQueryRepository,
			IEmailDomain emailDomain,
			IPasswordHasher passwordHasher,
			IUserLoginHistoryQueryRepository userLoginHistoryQueryRepository,
			IAuthenticationDataProvider authenticationDataProvider,
			IConfiguration configuration,
			ITranscriptFileQueryRepository transcriptFileQueryRepository,
			IFileProviderFactory fileProviderFactory,
			ITranscriptQueryRepository transcriptQueryRepository,
			ITranscriptDialogCommandRepository transcriptDialogCommandRepository,
			ITranscriptPersonCommandRepository transcriptPersonCommandRepository,
			ICourtCommandRepository courtCommandRepository,
			ITranscriptJobCommandRepository transcriptJobCommandRepository,
			ITranscriptRecordingInfoCommandRepository transcriptRecordingInfoCommandRepository,
			ITranscriptCommandRepository transcriptCommandRepository,
			ITranscriptionEngineQueryRepository transcriptionEngineQueryRepository)
		{
			UserCommandRepository = userCommandRepository;
			UserQueryRepository = userQueryRepository;
			RoleQueryRepository = roleQueryRepository;
			EmailDomain = emailDomain;
			PasswordHasher = passwordHasher;
			UserLoginHistoryQueryRepository = userLoginHistoryQueryRepository;
			AuthenticationDataProvider = authenticationDataProvider;
			Configuration = configuration;
			TranscriptFileQueryRepository = transcriptFileQueryRepository;
			FileProviderFactory = fileProviderFactory;
			TranscriptQueryRepository = transcriptQueryRepository;
			TranscriptDialogCommandRepository = transcriptDialogCommandRepository;
			TranscriptPersonCommandRepository = transcriptPersonCommandRepository;
			CourtCommandRepository = courtCommandRepository;
			TranscriptJobCommandRepository = transcriptJobCommandRepository;
			TranscriptRecordingInfoCommandRepository = transcriptRecordingInfoCommandRepository;
			TranscriptCommandRepository = transcriptCommandRepository;
			TranscriptionEngineQueryRepository = transcriptionEngineQueryRepository;
		}
    }
}
