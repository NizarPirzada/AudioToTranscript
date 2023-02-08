using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ITranscriptPersonQueryRepository
    {
        Task<IEnumerable<TranscriptPerson>> GetAllTranscriptPersonsByTranscriptIdAsync(int transcriptId);
        Task<TranscriptPerson> GeTranscriptPersonByIdAsync(int personId);
    }
}
