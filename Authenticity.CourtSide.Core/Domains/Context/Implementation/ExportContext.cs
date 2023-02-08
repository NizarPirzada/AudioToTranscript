using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Repositories.Query;

namespace Authenticity.CourtSide.Core.Domains.Context.Implementation
{
    public class ExportContext : IExportContext
    {
        public ITranscriptQueryRepository TranscriptQueryRepository { get; }
        public ITranscriptPersonQueryRepository TranscriptPersonQueryRepository { get; }
        public ITranscriptDialogQueryRepository TranscriptDialogQueryRepository { get; }
        public IFileExportFormatQueryRepository FileExportFormatQueryRepository { get; }
        public IFileExporter FileExporter { get; }
        public ICourtQueryRepository CourtQueryRepository { get; }
        public IFileProviderFactory FileProviderFactory { get; }
        public ITranslationEngine TranslationEngine { get; }
        public IUserQueryRepository UserQueryRepository { get; }
        public LocalFileHelper LocalFileHelper { get; }
		public ILanguageQueryRepository LanguageQueryRepository { get; }

		public ExportContext(ITranscriptQueryRepository transcriptQueryRepository,
            ITranscriptPersonQueryRepository transcriptPersonQueryRepository,
            ITranscriptDialogQueryRepository transcriptDialogQueryRepository,
            IFileExportFormatQueryRepository fileExportFormatQueryRepository,
            IFileExporter fileExporter,
            ICourtQueryRepository courtQueryRepository,
            IFileProviderFactory fileProviderFactory,
            ITranslationEngine translationEngine,
            IUserQueryRepository userQueryRepository,
            LocalFileHelper localFileHelper,
            ILanguageQueryRepository languageQueryRepository)
        {
            TranscriptQueryRepository = transcriptQueryRepository;
            TranscriptPersonQueryRepository = transcriptPersonQueryRepository;
            TranscriptDialogQueryRepository = transcriptDialogQueryRepository;
            FileExportFormatQueryRepository = fileExportFormatQueryRepository;
            FileExporter = fileExporter;
            CourtQueryRepository = courtQueryRepository;
            FileProviderFactory = fileProviderFactory;
            TranslationEngine = translationEngine;
            UserQueryRepository = userQueryRepository;
            LocalFileHelper = localFileHelper;
			LanguageQueryRepository = languageQueryRepository;
		}
    }
}
