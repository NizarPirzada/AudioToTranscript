using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;
using System.Transactions;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
	public class TranscriptCommandRepository : ITranscriptCommandRepository
	{
		public TranscriptCommandRepository(IDapperBase<Transcript> dapperTranscript)
		{
			DapperTranscript = dapperTranscript;
		}

		private IDapperBase<Transcript> DapperTranscript { get; set; }

		public Task<int> CreateTranscriptWithNameAsync(Transcript transcriptDto)
		{
			var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_CreateWithName, transcriptDto);
			return result;
		}

		public Task<int> UpdateTranscriptAsync(Transcript transcriptDto)
		{
			var result = DapperTranscript.EditAsync(SqlStatement.Transcript_UpdateTranscriptBasicInfo, transcriptDto);
			return result;
		}

		public Task<int> UpdateTranscriptTextAsync(int transcriptId, string text)
		{
			var parameters = new { Id = transcriptId, Transcription = text };
			var result = DapperTranscript.EditAsync(SqlStatement.Transcript_UpdateTranscriptText, parameters);
			return result;
		}

		public Task<int> DeleteAsync(int transcriptId)
		{
			var parameters = new { transcriptId };
			return DapperTranscript.DeleteAsync(SqlStatement.Transcript_Delete, parameters);
		}

		public Task<int> LockTranscriptAsync(LockTranscriptDto lockTranscriptDto)
		{
			return DapperTranscript.EditAsync(SqlStatement.Transcript_LockById, lockTranscriptDto);
		}
	}
}
