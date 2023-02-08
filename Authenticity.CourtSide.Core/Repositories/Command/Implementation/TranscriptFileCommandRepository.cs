using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class TranscriptFileCommandRepository : ITranscriptFileCommandRepository
    {
        public TranscriptFileCommandRepository(IDapperBase<TranscriptFile> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        private IDapperBase<TranscriptFile> DapperTranscript { get; set; }

        public Task<int> SaveFileAsync(TranscriptFile fileDto)
        {
            var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_CreateMediaFile, fileDto);            
            return result;
        }

        public Task<int> DeleteDuplicatedFileAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var result = DapperTranscript.DeleteAsync(SqlStatement.Transcript_DeleteDuplicatedFiles, parameters);
            return result;
        }

        public Task<int> DeleteAllFilesByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var rowsAffected = DapperTranscript.DeleteAsync(SqlStatement.Transcript_DeleteFilesByTranscriptId, parameters);
            return rowsAffected;
        }
    }
}
