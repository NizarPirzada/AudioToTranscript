using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
	public class TranscriptRecordingInfoQueryRepository : ITranscriptRecordingInfoQueryRepository
    {
        private IDapperBase<TranscriptRecordingInfoModel> DapperTranscriptRecordingInfo { get; set; }

        public TranscriptRecordingInfoQueryRepository(IDapperBase<TranscriptRecordingInfoModel> dapperTranscriptRecordingInfo)
        {
	        DapperTranscriptRecordingInfo = dapperTranscriptRecordingInfo;
        }

		public Task<TranscriptRecordingInfoModel> GetTranscriptRecordingInfoByIdAsync(int transcriptRecordingInfoId)
		{
			var parameters = new { id = transcriptRecordingInfoId };
			return DapperTranscriptRecordingInfo.GetByIdAsync(SqlStatement.RecordingInfo_GetById, parameters);
		}

		public Task<TranscriptRecordingInfoModel> GetTranscriptRecordingInfoByTranscriptIdAsync(int transcriptId)
		{
			var parameters = new { transcriptId };
			return DapperTranscriptRecordingInfo.GetByIdAsync(SqlStatement.RecordingInfo_GetByTranscriptId, parameters);
		}
	}
}
