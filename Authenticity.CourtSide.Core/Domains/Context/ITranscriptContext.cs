using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Authenticity.CourtSide.Core.Utilities.SignarlR;

namespace Authenticity.CourtSide.Core.Domains.Context
{
    public interface  ITranscriptContext
    {
        ITranscriptQueryRepository TranscriptQueryRepository { get; }
        ITranscriptPersonQueryRepository TranscriptPersonQueryRepository { get; }
        ITranscriptCommandRepository TranscriptCommandRepository { get; }
        ITranscriptPersonCommandRepository TranscriptPersonCommandRepository { get; }
        ITranscriptFileQueryRepository TranscriptFileQueryRepository { get; }
        ITranscriptFileCommandRepository TranscriptFileCommandRepository { get; }
        ITranscriptJobQueryRepository TranscriptJobQueryRepository { get; }
        ITranscriptJobCommandRepository TranscriptJobCommandRepository { get; }
        ITranscriptDialogQueryRepository TranscriptDialogQueryRepository { get; }
        ITranscriptDialogCommandRepository TranscriptDialogCommandRepository { get; }
        ICourtCommandRepository CourtCommandRepository { get; }
        IPersonAdditionalInfoCommandRepository PersonAdditionalInfoCommandRepository { get; }
        IFileProviderFactory FileProviderFactory { get; }
        IUserQueryRepository UserQueryRepository { get; }
        ITranscriptRecordingInfoCommandRepository TranscriptRecordingInfoCommandRepository { get; }
        ITranscriptionNotificationHub TranscriptionNotificationHub { get; }
        ILanguageQueryRepository LanguageQueryRepository { get; }
        ITranscriptionEngineQueryRepository TranscriptionEngineQueryRepository { get; }
    }
}
