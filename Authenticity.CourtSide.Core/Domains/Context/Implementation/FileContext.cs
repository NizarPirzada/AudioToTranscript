using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.Domains.Context.Implementation
{
    public class FileContext : IFileContext
    {
        public LocalFileHelper LocalFileHelper { get; }
        public ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        public IConfiguration Configuration { get; }
        public IFileProviderFactory FileProviderFactory { get; }
        public ITranscriptQueryRepository TranscriptQueryRepository { get; }
        public IUserQueryRepository UserQueryRepository { get; }
        public ITranscriptFileCommandRepository TranscriptFileCommandRepository { get; }
        public ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        public ITranscriptCommandRepository TranscriptCommandRepository { get; }

        public FileContext(
            LocalFileHelper localFileHelper,
            ITranscriptFileQueryRepository transcriptFileQueryRepository,
            IConfiguration configuration,
            IFileProviderFactory fileProviderFactory,
            ITranscriptQueryRepository transcriptQueryRepository,
            IUserQueryRepository userQueryRepository,
            ITranscriptFileCommandRepository transcriptFileCommandRepository,
            ITranscriptJobCommandRepository transcriptJobCommandRepository,
            ITranscriptCommandRepository transcriptCommandRepository)
        {
            LocalFileHelper = localFileHelper;
            TranscriptFileQueryRepository = transcriptFileQueryRepository;
            Configuration = configuration;
            FileProviderFactory = fileProviderFactory;
            TranscriptQueryRepository = transcriptQueryRepository;
            UserQueryRepository = userQueryRepository;
            TranscriptFileCommandRepository = transcriptFileCommandRepository;
            TranscriptJobCommandRepository = transcriptJobCommandRepository;
            TranscriptCommandRepository = transcriptCommandRepository;
        }
    }
}
