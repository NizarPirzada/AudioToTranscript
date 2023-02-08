using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class TranscriptDialogQueryRepository : ITranscriptDialogQueryRepository
    {
        private IDapperBase<TranscriptDialog> DapperTranscript { get; set; }

        public TranscriptDialogQueryRepository(IDapperBase<TranscriptDialog> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        public Task<IEnumerable<TranscriptDialog>> GetAllTranscriptDialogByIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetAllDialogsByTranscriptId, parameters);
            return transcriptResult;
        }

        public Task<IEnumerable<TranscriptDialog>> GetAllTranscriptDialogByIdAndPaginationAsync(int transcriptId, int page)
        {
            var parameters = new { transcriptId, page };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetAllDialogsByTranscriptIdAndPage, parameters);
            return transcriptResult;
        }
    }
}
