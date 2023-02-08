using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.Domains.Context
{
    public interface IFileContext
    {
        LocalFileHelper LocalFileHelper { get; }
        ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        IConfiguration Configuration { get; }
        IFileProviderFactory FileProviderFactory { get; }
        ITranscriptQueryRepository TranscriptQueryRepository { get; }
        IUserQueryRepository UserQueryRepository { get; }
        ITranscriptFileCommandRepository TranscriptFileCommandRepository { get; }
        ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        ITranscriptCommandRepository TranscriptCommandRepository { get; }
    }
}
