using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ITranscriptFileCommandRepository
    {
        Task<int> SaveFileAsync(TranscriptFile fileDto);
        Task<int> DeleteDuplicatedFileAsync(int transcriptId);
        Task<int> DeleteAllFilesByTranscriptIdAsync(int transcriptId);
    }
}
