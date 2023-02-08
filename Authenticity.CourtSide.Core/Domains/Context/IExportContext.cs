using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Repositories.Query;

namespace Authenticity.CourtSide.Core.Domains.Context
{
    public interface IExportContext
    {
        ITranscriptQueryRepository TranscriptQueryRepository { get; }
        ITranscriptPersonQueryRepository TranscriptPersonQueryRepository { get; }
        ITranscriptDialogQueryRepository TranscriptDialogQueryRepository { get; }
        IFileExportFormatQueryRepository FileExportFormatQueryRepository { get; }
        IFileExporter FileExporter { get; }
        IFileProviderFactory FileProviderFactory { get; }
        ITranslationEngine TranslationEngine { get; }
        IUserQueryRepository UserQueryRepository { get; }
        LocalFileHelper LocalFileHelper { get; }
        ILanguageQueryRepository LanguageQueryRepository { get; }
    }
}
