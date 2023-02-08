using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ITranscriptFileQueryRepository
    {
        Task<IEnumerable<TranscriptFile>> GetAllTranscriptFileByIdAsync(int transcriptId);
        Task<IEnumerable<TranscriptFile>> GetAllTranscriptFileByUserIdAsync(int userId);
    }
}
