using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ITranscriptQueryRepository
    {
        Task<Transcript> GetTranscriptByIdAsync(int transcriptId);
        Task<IEnumerable<Transcript>> GetAllTranscriptsByUserAsync(int userId);
        Task<bool> ChekUserTranscriptAccessAsync(int userId, int transcriptId);
    }
}
