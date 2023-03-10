using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
	public interface ITranscriptionEngineQueryRepository
    {
        Task<TranscriptionEngine> GetTranscriptionEngineByIdAsync(int transcriptionEngineId);
        Task<IEnumerable<TranscriptionEngine>> GetAllTranscriptionEnginesAsync();
    }
}
