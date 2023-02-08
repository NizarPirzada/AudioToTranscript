using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class TranscriptQueryRepository : ITranscriptQueryRepository
    {
        private IDapperBase<Transcript> DapperTranscript { get; set; }

        public TranscriptQueryRepository(IDapperBase<Transcript> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        public async Task<Transcript> GetTranscriptByIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var transcriptResult = await DapperTranscript.GetByIdWithRelationsAsync<CourtModel, TranscriptRecordingInfoModel>(
                SqlStatement.Transcript_GetTranscriptById, MapTranscriptRelations,
                "CourtId,TranscriptRecordingInfoId", parameters);
            return transcriptResult.FirstOrDefault();
        }

        public Task<IEnumerable<Transcript>> GetAllTranscriptsByUserAsync(int userId)
        {
            var parameters = new { userId };
            var transcriptResult = DapperTranscript.GetAllAsync(SqlStatement.Transcript_GetAllByUser, parameters);
            return transcriptResult;
        }

        public Task<bool> ChekUserTranscriptAccessAsync(int userId, int transcriptId)
		{
            var parameters = new { userId, transcriptId };
            return DapperTranscript.GetAsync<bool>(SqlStatement.Transcript_CheckUser, parameters);
        }

        private Transcript MapTranscriptRelations(Transcript transcriptModel, CourtModel courtModel, TranscriptRecordingInfoModel recordingInfoModel, IDictionary<int, Transcript> transcriptRelations)
        {
            Transcript transcriptRow = ValidateDictionaryKey(transcriptModel, transcriptRelations);
            MapCourtInfo(courtModel, transcriptRow);
            MapRecordingInfo(recordingInfoModel, transcriptRow);
            return transcriptRow;
        }
        private void MapCourtInfo(CourtModel courtModel, Transcript transcriptModel)
        {
            if (courtModel != null)
            {
                transcriptModel.Court = courtModel;
            }
        }

        private void MapRecordingInfo(TranscriptRecordingInfoModel recordingInfoModel, Transcript transcriptModel)
        {
	        if (recordingInfoModel != null)
	        {
		        transcriptModel.TranscriptRecordingInfo = recordingInfoModel;
	        }
        }

        private Transcript ValidateDictionaryKey(Transcript transcriptModel, IDictionary<int, Transcript> transcriptRelations)
        {
            if (!transcriptRelations.TryGetValue(transcriptModel.Id, out Transcript transcriptRow))
            {
                transcriptRelations.Add(transcriptModel.Id, transcriptRow = transcriptModel);
            }

            return transcriptRow;
        }

    }
}
