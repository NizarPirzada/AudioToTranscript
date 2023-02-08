using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
	public class TranscriptionEngineQueryRepository : ITranscriptionEngineQueryRepository
	{
		public TranscriptionEngineQueryRepository(IDapperBase<TranscriptionEngine> dapperTranscriptionEngine)
		{
			DapperTranscriptionEngine = dapperTranscriptionEngine;
		}

		public IDapperBase<TranscriptionEngine> DapperTranscriptionEngine { get; }

		public Task<IEnumerable<TranscriptionEngine>> GetAllTranscriptionEnginesAsync()
		{
			return DapperTranscriptionEngine.GetAllAsync(SqlStatement.TranscriptionEngine_GetAll);
		}

		public Task<TranscriptionEngine> GetTranscriptionEngineByIdAsync(int transcriptionEngineId)
		{
			var parameters = new { transcriptionEngineId };
			return DapperTranscriptionEngine.GetByIdAsync(SqlStatement.TranscriptionEngine_GetById, parameters);
		}
	}
}
