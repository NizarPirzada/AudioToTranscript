using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class TranscriptPersonQueryRepository : ITranscriptPersonQueryRepository
    {
        private IDapperBase<TranscriptPerson> DapperTranscript { get; set; }

        public TranscriptPersonQueryRepository(IDapperBase<TranscriptPerson> dapperTranscript)
        {
            DapperTranscript = dapperTranscript;
        }

        public Task<IEnumerable<TranscriptPerson>> GetAllTranscriptPersonsByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var transcriptResult = DapperTranscript.GetByIdWithRelationsAsync<PersonAdditionalInformation>(
                SqlStatement.Transcript_GetAllPersonsByTranscriptId, MapPersonRelations, 
                "PersonAdditionalInformationId", 
                parameters
            );
            return transcriptResult;
        }

        public async Task<TranscriptPerson> GeTranscriptPersonByIdAsync(int personId)
        {
            var parameters = new { personId };
            var result = await DapperTranscript.GetByIdWithRelationsAsync<PersonAdditionalInformation>(
                SqlStatement.Transcript_GetPersonById, MapPersonRelations,
                "PersonAdditionalInformationId",
                parameters
            );

            return result.FirstOrDefault();
        }

        private TranscriptPerson MapPersonRelations(TranscriptPerson personModel, PersonAdditionalInformation infoModel, IDictionary<int, TranscriptPerson> personRelations)
        {
            TranscriptPerson userRow = ValidatePersonDictionaryKey(personModel, personRelations);
            MapAdditionalInfo(infoModel, userRow);
            return userRow;
        }
        private void MapAdditionalInfo(PersonAdditionalInformation infoModel, TranscriptPerson personRow)
        {
            if (infoModel != null)
            {
                personRow.AdditionalInfo = infoModel;
            }
        }
        private TranscriptPerson ValidatePersonDictionaryKey(TranscriptPerson personModel, IDictionary<int, TranscriptPerson> personRelations)
        {
            if (!personRelations.TryGetValue(personModel.Id, out TranscriptPerson personRow))
            {
                personRelations.Add(personModel.Id, personRow = personModel);
            }

            return personRow;
        }
    }
}
