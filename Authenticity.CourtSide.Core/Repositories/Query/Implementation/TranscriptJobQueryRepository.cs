using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class TranscriptJobQueryRepository : ITranscriptJobQueryRepository
    {
        private IDapperBase<TranscriptJob> DapperTranscript { get; set; }

        public TranscriptJobQueryRepository(IDapperBase<TranscriptJob> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        public Task<TranscriptJob> GetActiveJobByTranscriptAsync(int transcriptId)
        {
            var parameters = new { transcriptId = transcriptId };
            return DapperTranscript.GetAsync<TranscriptJob>(SqlStatement.Transcript_GetActiveJobByTranscriptId, parameters);
        }

        public Task<IEnumerable<TranscriptJob>> GetBatchOfJobsByStatusAsync(TranscriptJobStatusEnum jobStatus)
        {
            var parameters = new { status = jobStatus };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetBatchOfJobsByStatus, parameters);
            return transcriptResult;
        }
    }
}
