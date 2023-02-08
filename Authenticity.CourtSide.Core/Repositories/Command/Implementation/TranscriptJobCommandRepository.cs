using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class TranscriptJobCommandRepository : ITranscriptJobCommandRepository
    {
        public TranscriptJobCommandRepository(IDapperBase<TranscriptJob> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        private IDapperBase<TranscriptJob> DapperTranscript { get; set; }

        public Task<int> CreateJobAsync(TranscriptJob jobDto)
        {
            var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_CreateJob, jobDto);            
            return result;
        }

        public Task<bool> UpdateJobAsync(TranscriptJob jobDto)
        {
            return EditJobAsync(SqlStatement.Transcript_UpdateJob, jobDto);
        }

        public Task<bool> CancelJobsInProgressAsync(int transcriptId)
        {
            TranscriptJob jobDto = new TranscriptJob() { 
                TranscriptId = transcriptId
            };

            return EditJobAsync(SqlStatement.Transcript_CancelJobsInProgress, jobDto);
        }
        private async Task<bool> EditJobAsync(string sqlCommand, TranscriptJob jobDto)
        {
            var result = await DapperTranscript.EditAsync(sqlCommand, jobDto);
            return result != 0 ? true : false;
        }

        public Task<int> DeleteAllJobsByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new {transcriptId};
            return DapperTranscript.DeleteAsync(SqlStatement.Transcript_DeleteAllJobsByTranscriptId, parameters);
        }
    }
}
