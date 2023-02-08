using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;

namespace Authenticity.CourtSide.Core.Domains.Context
{
    public interface ITranscriptionProvidersDomainContext
    {
        ITranscriptJobQueryRepository TranscriptJobQueryRepository { get; }
        ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        ITranscriptDialogCommandRepository TranscriptDialogCommandRepository { get; }
        ITranscriptCommandRepository TranscriptCommandRepository { get; }
        ITranscriptionEngine TranscriptionEngine { get; }
        IFileProviderFactory FileProviderFactory { get; }
        IUserQueryRepository UserQueryRepository { get; }
        ITranscriptQueryRepository TranscriptQueryRepository { get; }
        ILanguageQueryRepository LanguageQueryRepository { get; }
    }
}
