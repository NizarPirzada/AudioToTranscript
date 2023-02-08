using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class TranscriptPersonCommandRepository : ITranscriptPersonCommandRepository
    {
        public TranscriptPersonCommandRepository(IDapperBase<TranscriptPerson> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        private IDapperBase<TranscriptPerson> DapperTranscript { get; set; }

        public Task<int> CreatePersonAsync(TranscriptPerson personDto)
        {
            var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_CreatePerson, personDto);
            return result;
        }

        public Task<int> UpdatePersonAsync(TranscriptPerson personDto)
        {
            var result = DapperTranscript.EditAsync(SqlStatement.Transcript_UpdatePerson, personDto);
            return result;
        }

        public Task<int> DeletePersonAsync(int personId)
        {
            var parameters = new { personId };
            return DapperTranscript.EditAsync(SqlStatement.Transcript_DeletePerson, parameters);
        }

        public Task<int> DeleteAllPeopleByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new {transcriptId};
            return DapperTranscript.DeleteAsync(SqlStatement.Transcript_DeleteAllPeopleByTranscriptId, parameters);
        }
    }
}
