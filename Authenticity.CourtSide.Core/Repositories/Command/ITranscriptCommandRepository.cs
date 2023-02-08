using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ITranscriptCommandRepository
    {
        Task<int> CreateTranscriptWithNameAsync(Transcript transcriptDto);
        Task<int> UpdateTranscriptAsync(Transcript transcriptDto);
        Task<int> UpdateTranscriptTextAsync(int transcriptId, string text);
        Task<int> DeleteAsync(int transcriptId);
        Task<int> LockTranscriptAsync(LockTranscriptDto lockTranscriptDto);
    }
}
