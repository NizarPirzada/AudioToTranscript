using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ITranscriptDialogQueryRepository
    {
        Task<IEnumerable<TranscriptDialog>> GetAllTranscriptDialogByIdAsync(int transcriptId);
        Task<IEnumerable<TranscriptDialog>> GetAllTranscriptDialogByIdAndPaginationAsync(int transcriptId, int page);
    }
}
