using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;

namespace Authenticity.CourtSide.Core.Domains.Context.Implementation
{
    public class TranscriptionProvidersDomainContext : ITranscriptionProvidersDomainContext
    {
        public ITranscriptJobQueryRepository TranscriptJobQueryRepository { get; }
        public ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        public ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        public ITranscriptDialogCommandRepository TranscriptDialogCommandRepository { get; }
        public ITranscriptCommandRepository TranscriptCommandRepository { get; }
        public ITranscriptionEngine TranscriptionEngine { get; }
        public IFileProviderFactory FileProviderFactory { get; }
        public IUserQueryRepository UserQueryRepository { get; }
		public ITranscriptQueryRepository TranscriptQueryRepository { get; }
		public ILanguageQueryRepository LanguageQueryRepository { get; }

		public TranscriptionProvidersDomainContext(ITranscriptJobQueryRepository transcriptJobQueryRepository,
            ITranscriptFileQueryRepository transcriptFileQueryRepository,
            ITranscriptJobCommandRepository transcriptJobCommandRepository,
            ITranscriptDialogCommandRepository transcriptDialogCommandRepository,
            ITranscriptCommandRepository transcriptCommandRepository,
            ITranscriptionEngine transcriptionEngine,
            IFileProviderFactory fileProviderFactory,
            IUserQueryRepository userQueryRepository,
            ITranscriptQueryRepository transcriptQueryRepository,
            ILanguageQueryRepository languageQueryRepository)
        {
            TranscriptJobQueryRepository = transcriptJobQueryRepository;
            TranscriptFileQueryRepository = transcriptFileQueryRepository;
            TranscriptJobCommandRepository = transcriptJobCommandRepository;
            TranscriptDialogCommandRepository = transcriptDialogCommandRepository;
            TranscriptCommandRepository = transcriptCommandRepository;
            TranscriptionEngine = transcriptionEngine;
            FileProviderFactory = fileProviderFactory;
            UserQueryRepository = userQueryRepository;
			TranscriptQueryRepository = transcriptQueryRepository;
			LanguageQueryRepository = languageQueryRepository;
		}
    }
}
