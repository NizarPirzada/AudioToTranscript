using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ITranscriptJobCommandRepository
    {
        Task<int> CreateJobAsync(TranscriptJob jobDto);
        Task<bool> UpdateJobAsync(TranscriptJob jobDto);
        Task<bool> CancelJobsInProgressAsync(int transcriptId);
        Task<int> DeleteAllJobsByTranscriptIdAsync(int transcriptId);
    }
}
