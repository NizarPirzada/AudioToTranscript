using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
	public interface ITranscriptRecordingInfoQueryRepository
    {
        Task<TranscriptRecordingInfoModel> GetTranscriptRecordingInfoByIdAsync(int transcriptRecordingInfoId);
        Task<TranscriptRecordingInfoModel> GetTranscriptRecordingInfoByTranscriptIdAsync(int transcriptId);
    }
}
