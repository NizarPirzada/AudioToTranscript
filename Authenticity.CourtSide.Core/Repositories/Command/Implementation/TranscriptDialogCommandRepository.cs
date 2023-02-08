using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
	public class TranscriptDialogCommandRepository : ITranscriptDialogCommandRepository
	{
		public TranscriptDialogCommandRepository(IDapperBase<TranscriptDialog> dapperTranscript, IDapperBase<ChangeSpeakerDto> dapperMassiveOperations)
		{
			DapperTranscript = dapperTranscript;
			DapperMassiveOperations = dapperMassiveOperations;
		}

		private IDapperBase<TranscriptDialog> DapperTranscript { get; set; }
		private IDapperBase<ChangeSpeakerDto> DapperMassiveOperations { get; set; }

		public Task<int> CreateDialogAsync(TranscriptDialog dialogDto)
		{
			var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_CreateDialog, dialogDto);
			return result;
		}

		public Task<int> UpdateDialogAsync(TranscriptDialog dialogDto)
		{
			var result = DapperTranscript.SaveAsync(SqlStatement.Transcript_UpdateDialog, dialogDto);
			return result;
		}

		public Task<int> UpdateAllSpeakersAsync(ChangeSpeakerDto speakerRequest)
		{
			return DapperMassiveOperations.SaveAsync(SqlStatement.Transcript_UpdateAllSpeakers, speakerRequest);
		}

		public Task<int> UpdateExaminationBlockAsync(TranscriptDialog dialogFrom, TranscriptDialog dialogTo)
		{
			var parameters = new
			{
				transcriptId = dialogFrom.TranscriptId,
				examinationType = dialogFrom.ExaminationType,
				startId = dialogFrom.Id,
				endId = dialogTo.Id,
				examinationTag = dialogFrom.ExaminationTag
			};
			var result = DapperMassiveOperations.SaveAsync(SqlStatement.Transcript_SaveExamination, parameters);
			return result;
		}

		public Task<int> DeleteDialogsByTranscriptIdAsync(int transcriptId)
		{
			var parameters = new { transcriptId };
			return DapperTranscript.DeleteAsync(SqlStatement.Transcript_DeleteDialogsByTranscriptId, parameters);
		}

		public Task<int> UpdateSingleExaminationTagAsync(TranscriptDialog transcriptDialog)
		{
			return DapperTranscript.SaveAsync(SqlStatement.Transcript_UpdateSingleExaminationTag, transcriptDialog);
		}

		public Task<int> UpdateSingleExaminationAsync(TranscriptDialog transcriptDialog)
		{
			return DapperTranscript.SaveAsync(SqlStatement.Transcript_UpdateSingleExamination, transcriptDialog);
		}

		public Task<int> UpdateMassivelyExaminationTagAsync(TranscriptDialog dialogFrom, TranscriptDialog dialogTo)
		{
			var parameters = new
			{
				transcriptId = dialogFrom.TranscriptId,
				startId = dialogFrom.Id,
				originalSpeakerId = dialogFrom.OriginalSpeakerId,
				examinationTag = dialogFrom.ExaminationTag,
				endId = dialogTo.Id
			};

			return DapperTranscript.SaveAsync(SqlStatement.Transcript_UpdateMassiveExaminationTag, parameters);
		}
	}
}
