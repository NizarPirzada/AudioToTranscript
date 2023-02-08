using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
	public class TranscriptRecordingInfoCommandRepository : ITranscriptRecordingInfoCommandRepository
	{
		private IDapperBase<TranscriptRecordingInfoModel> DapperTranscriptRecordingInfo { get; }

		public TranscriptRecordingInfoCommandRepository(IDapperBase<TranscriptRecordingInfoModel> dapperTranscriptRecordingInfo)
		{
			DapperTranscriptRecordingInfo = dapperTranscriptRecordingInfo;
		}

		public Task<int> CreateTranscriptRecordingInfoAsync(TranscriptRecordingInfoModel transcriptRecordingInfoModel)
		{
			return DapperTranscriptRecordingInfo.SaveAsync(SqlStatement.RecordingInfo_Create, transcriptRecordingInfoModel);
		}

		public Task<int> UpdateTranscriptRecordingInfoAsync(TranscriptRecordingInfoModel transcriptRecordingInfoModel)
		{
			return DapperTranscriptRecordingInfo.EditAsync(SqlStatement.RecordingInfo_Update, transcriptRecordingInfoModel);
		}

		public Task<int> DeleteTranscriptRecordingInfoByTranscriptIdAsync(int transcriptId)
		{
			var parameters = new { transcriptId };
			return DapperTranscriptRecordingInfo.DeleteAsync(SqlStatement.RecordingInfo_DeleteByTranscriptId, parameters);
		}
	}
}
