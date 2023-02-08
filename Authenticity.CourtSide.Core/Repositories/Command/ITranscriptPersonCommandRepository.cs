using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ITranscriptPersonCommandRepository
    {
        Task<int> CreatePersonAsync(TranscriptPerson personDto);
        Task<int> UpdatePersonAsync(TranscriptPerson personDto);
        Task<int> DeletePersonAsync(int personId);
        Task<int> DeleteAllPeopleByTranscriptIdAsync(int transcriptId);
    }
}
