using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class CourtQueryRepository : ICourtQueryRepository
    {
        private IDapperBase<CourtModel> DapperCourt { get; set; }

        public CourtQueryRepository(IDapperBase<CourtModel> dapperCourt)
        {
            DapperCourt = dapperCourt;
        }

        public Task<CourtModel> GetCourtByIdAsync(int courtId)
        {
            var parameters = new { id = courtId };
            var courtResult = DapperCourt.GetByIdAsync(SqlStatement.Court_GetById, parameters);
            return courtResult;
        }

        public Task<CourtModel> GetCourtByTranscriptIdAsync(int transcriptId)
        {
            var parameters = new { transcriptId };
            var courtResult = DapperCourt.GetByIdAsync(SqlStatement.Court_GetByTranscriptId, parameters);
            return courtResult;
        }
    }
}
