using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
	public interface ITranscriptRecordingInfoCommandRepository
	{
		Task<int> CreateTranscriptRecordingInfoAsync(TranscriptRecordingInfoModel transcriptRecordingInfoModel);
		Task<int> UpdateTranscriptRecordingInfoAsync(TranscriptRecordingInfoModel transcriptRecordingInfoModel);
		Task<int> DeleteTranscriptRecordingInfoByTranscriptIdAsync(int transcriptId);
	}
}
