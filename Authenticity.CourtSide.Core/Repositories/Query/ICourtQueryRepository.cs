using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ICourtQueryRepository
    {
        Task<CourtModel> GetCourtByIdAsync(int courtId);
        Task<CourtModel> GetCourtByTranscriptIdAsync(int transcriptId);
    }
}
