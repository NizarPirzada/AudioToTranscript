using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
	public interface ITranscriptDialogCommandRepository
    {
        Task<int> CreateDialogAsync(TranscriptDialog dialogDto);
        Task<int> UpdateDialogAsync(TranscriptDialog dialogDto);
        Task<int> UpdateAllSpeakersAsync(ChangeSpeakerDto speakerRequest);
        Task<int> DeleteDialogsByTranscriptIdAsync(int transcriptId);
        Task<int> UpdateExaminationBlockAsync(TranscriptDialog dialogFrom, TranscriptDialog dialogTo);
        Task<int> UpdateSingleExaminationAsync(TranscriptDialog transcriptDialog);
        Task<int> UpdateSingleExaminationTagAsync(TranscriptDialog transcriptDialog);
        Task<int> UpdateMassivelyExaminationTagAsync(TranscriptDialog dialogFrom, TranscriptDialog dialogTo);
    }
}
