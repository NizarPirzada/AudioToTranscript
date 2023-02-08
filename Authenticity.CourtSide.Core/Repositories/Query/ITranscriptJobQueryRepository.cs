using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ITranscriptJobQueryRepository
    {
        Task<TranscriptJob> GetActiveJobByTranscriptAsync(int transcriptId);
        Task<IEnumerable<TranscriptJob>> GetBatchOfJobsByStatusAsync(TranscriptJobStatusEnum jobStatus);
    }
}
