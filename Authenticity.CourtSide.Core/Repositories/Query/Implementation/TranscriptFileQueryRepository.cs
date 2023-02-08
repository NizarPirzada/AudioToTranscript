using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class TranscriptFileQueryRepository : ITranscriptFileQueryRepository
    {
        private IDapperBase<TranscriptFile> DapperTranscript { get; set; }

        public TranscriptFileQueryRepository(IDapperBase<TranscriptFile> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        public Task<IEnumerable<TranscriptFile>> GetAllTranscriptFileByIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetAllFilesByTranscriptId, parameters);
            return transcriptResult;
        }

        public Task<IEnumerable<TranscriptFile>> GetAllTranscriptFileByUserIdAsync(int userId)
        {
            var parameters = new { userId };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetAllFilesByUserId, parameters);
            return transcriptResult;
        }
    }
}
