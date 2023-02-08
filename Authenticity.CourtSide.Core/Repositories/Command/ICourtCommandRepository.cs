using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ICourtCommandRepository
    {
        Task<int> CreateCourtAsync(CourtModel courtDto);
        Task<int> UpdateCourtAsync(CourtModel courtDto);
        Task<int> DeleteCourtByTranscriptIdAsync(int transcriptId);
    }
}
