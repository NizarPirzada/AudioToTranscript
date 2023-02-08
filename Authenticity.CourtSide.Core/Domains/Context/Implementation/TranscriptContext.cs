using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Authenticity.CourtSide.Core.Utilities.SignarlR;

namespace Authenticity.CourtSide.Core.Domains.Context.Implementation
{
	public class TranscriptContext : ITranscriptContext
    {
        public ITranscriptQueryRepository TranscriptQueryRepository { get; }
        public ITranscriptPersonQueryRepository TranscriptPersonQueryRepository { get; }
        public ITranscriptCommandRepository TranscriptCommandRepository { get; }
        public ITranscriptPersonCommandRepository TranscriptPersonCommandRepository { get; }
        public ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        public ITranscriptFileCommandRepository TranscriptFileCommandRepository { get; }
        public ITranscriptJobQueryRepository TranscriptJobQueryRepository { get;  }
        public ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        public ITranscriptDialogQueryRepository TranscriptDialogQueryRepository { get; }
        public ITranscriptDialogCommandRepository TranscriptDialogCommandRepository {get; }
        public ICourtCommandRepository CourtCommandRepository { get; }
        public IPersonAdditionalInfoCommandRepository PersonAdditionalInfoCommandRepository { get; }
        public IFileProviderFactory FileProviderFactory { get; }
        public IUserQueryRepository UserQueryRepository { get; }
        public ITranscriptRecordingInfoCommandRepository TranscriptRecordingInfoCommandRepository { get; }
		public ITranscriptionNotificationHub TranscriptionNotificationHub { get; }
		public ILanguageQueryRepository LanguageQueryRepository { get; }
		public ITranscriptionEngineQueryRepository TranscriptionEngineQueryRepository { get; }

		public TranscriptContext(ITranscriptQueryRepository transcriptQueryRepository,
            ITranscriptCommandRepository transcriptCommandRepository,
            ITranscriptPersonQueryRepository transcriptPersonQueryRepository,
            ITranscriptPersonCommandRepository transcriptPersonCommandRepository,
            ITranscriptFileQueryRepository transcriptFileQueryRepository,
            ITranscriptFileCommandRepository transcriptFileCommandRepository,
            ITranscriptJobCommandRepository transcriptJobCommandRepository,
            ITranscriptDialogQueryRepository transcriptDialogQueryRepository,
            ITranscriptDialogCommandRepository transcriptDialogCommandRepository,
            ICourtCommandRepository courtCommandRepository,
            IPersonAdditionalInfoCommandRepository personAdditionalInfoCommandRepository,
            ITranscriptJobQueryRepository transcriptJobQueryRepository,
            IFileProviderFactory fileProviderFactory,
            IUserQueryRepository userQueryRepository,
            ITranscriptRecordingInfoCommandRepository transcriptRecordingInfoCommandRepository,
            ITranscriptionNotificationHub transcriptionNotificationHub,
            ILanguageQueryRepository languageQueryRepository,
            ITranscriptionEngineQueryRepository transcriptionEngineQueryRepository)
        {
            TranscriptQueryRepository = transcriptQueryRepository;
            TranscriptCommandRepository = transcriptCommandRepository;
            TranscriptPersonQueryRepository = transcriptPersonQueryRepository;
            TranscriptPersonCommandRepository = transcriptPersonCommandRepository;
            TranscriptFileQueryRepository = transcriptFileQueryRepository;
            TranscriptFileCommandRepository = transcriptFileCommandRepository;
            TranscriptJobCommandRepository = transcriptJobCommandRepository;
            TranscriptDialogQueryRepository = transcriptDialogQueryRepository;
            TranscriptDialogCommandRepository = transcriptDialogCommandRepository;
            CourtCommandRepository = courtCommandRepository;
            PersonAdditionalInfoCommandRepository = personAdditionalInfoCommandRepository;
            TranscriptJobQueryRepository = transcriptJobQueryRepository;
            FileProviderFactory = fileProviderFactory;
            UserQueryRepository = userQueryRepository;
            TranscriptRecordingInfoCommandRepository = transcriptRecordingInfoCommandRepository;
			TranscriptionNotificationHub = transcriptionNotificationHub;
            LanguageQueryRepository = languageQueryRepository;
			TranscriptionEngineQueryRepository = transcriptionEngineQueryRepository;
		}
    }
}
