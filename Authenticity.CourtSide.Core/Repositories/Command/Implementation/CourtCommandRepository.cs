using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class CourtCommandRepository : ICourtCommandRepository
    {
        public CourtCommandRepository(IDapperBase<CourtModel> dapperCourt)
        {
            DapperCourt = dapperCourt;
        }

        private IDapperBase<CourtModel> DapperCourt { get; set; }

        public Task<int> CreateCourtAsync(CourtModel courtDto)
        {
            var result = DapperCourt.SaveAsync(SqlStatement.Court_Create, courtDto);
            return result;
        }

        public Task<int> UpdateCourtAsync(CourtModel courtDto)
        {
            var result = DapperCourt.EditAsync(SqlStatement.Court_Update, courtDto);
            return result;
        }

        public Task<int> DeleteCourtByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new {transcriptId};
            return DapperCourt.DeleteAsync(SqlStatement.Court_DeleteByTranscriptId, parameters);
        }
    }
}
