using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface ITranscriptDomain : IDomain
    {
		/// <summary>
		/// Get All transcripts created by a user.
		/// </summary>
		/// <param name="userId">User ID to filter the transcripts</param>
		/// <returns>List of transcripts created by the user ID provided.</returns>
		Task<IEnumerable<Transcript>> GetAllTranscriptsByUserAsync(int userId);

		Task<Transcript> GetTranscriptByIdAsync(int id);

		/// <summary>
		/// CreateAsync a transcript only with the name as required parameter.
		/// </summary>
		/// <param name="transcriptDto">Transcript object with the name.</param>
		/// <returns>Transcript object created.</returns>
		Task<Transcript> CreateSimpleTranscriptAsync(Transcript transcriptDto);

		/// <summary>
		/// Update teh basic information of a transcript.
		/// </summary>
		/// <param name="transcriptDto">Transcript object with new information.</param>
		/// <returns>Transcript object updated.</returns>
		Task<Transcript> UpdateTranscriptAsync(Transcript transcriptDto);

		/// <summary>
		/// Get the list of all persons linked to a Transcript.
		/// </summary>
		/// <param name="transcriptId">Transcript id to get the persons.</param>
		/// <returns>List of persons linked to the transcript ID provided.</returns>
		Task<IEnumerable<TranscriptPerson>> GetAllTranscriptPersonByIdAsync(int transcriptId);

		/// <summary>
		/// CreateAsync a person linked to a Transcript.
		/// </summary>
		/// <param name="person">Person information.</param>
		/// <returns>ID of person created.</returns>
		Task<TranscriptPerson> CreateTranscriptPersonAsync(TranscriptPerson person);

		/// <summary>
		/// Update person's information.
		/// </summary>
		/// <param name="person">Person information.</param>
		/// <returns>ID of person updated.</returns>
		Task<TranscriptPerson> UpdateTranscriptPersonAsync(TranscriptPerson person);

		/// <summary>
		/// CreateAsync a file linked to a Transcript.
		/// </summary>
		/// <param name="file">File information.</param>
		/// <returns>ID of person created.</returns>
		Task<Transcript> SaveTranscriptMediaInformationAsync(TranscriptFile file);

		/// <summary>
		/// Get the  a file linked to a Transcript.
		/// </summary>
		/// <param name="transcriptId">Transcript id.</param>
		/// <returns>List of file's metadata linked to a Transcript.</returns>
		Task<IEnumerable<TranscriptFile>> GetTranscriptMediaFileListByIdAsync(int transcriptId);

		/// <summary>
		/// Get all the dialogs linked to a transcript.
		/// </summary>
		/// <param name="transcriptId">Transcript id.</param>
		/// <param name="page">The number page to return the dialogs.</param>
		/// <returns>List of dialogss linked to a Transcript.</returns>
		Task<IEnumerable<TranscriptDialog>> GetTranscriptDialogsByIdAndPaginationAsync(int transcriptId, int page);

		/// <summary>
		/// Update the dialog information (speaker and transcription).
		/// </summary>
		/// <param name="transcriptId">Transcript id.</param>
		/// <returns>List of dialogss linked to a Transcript.</returns>
		Task<int> UpdateTranscriptDialogAsync(TranscriptDialog dialog);

		/// <summary>
		/// Change the speaker for all dialogs ith the same Original speaker ID.
		/// </summary>
		/// <param name="dialog">Request DTO with the information to perform the massive update.</param>
		/// <returns>Number of records  of dialogss linked to a Transcript.</returns>
		Task<int> UpdateAllSpeakersAsync(ChangeSpeakerDto speakerRequest);

		/// <summary>
		/// Delete the transcript person information
		/// </summary>
		/// <param name="transcriptPersonId">Transcript person Id</param>
		/// <returns>Number of records affected</returns>
        Task<int> DeleteTranscriptPersonAsync(int transcriptPersonId);
    
		/// <summary>
		/// Get the current active job for a Transcription
		/// </summary>
		/// <param name="transcriptId">Id of the transcript.</param>
		/// <returns>Current job status.</returns>
		Task<TranscriptionStatus> GetActiveTrancriptJobAsync(int transcriptId);

		/// <summary>
		/// Cancel all current jobs and crate a new one.
		/// </summary>
		/// <param name="transcriptId">Id of the transcript which jobs will be canceled.</param>
		/// <returns>Result of the operation.</returns>
		Task<bool> ResendTranscriptJobAsync(TranscriptJob transcriptJob);

		/// <summary>
		/// Delete the transcript with all associated objects and files
		/// </summary>
		/// <param name="transcriptId"></param>
		/// <returns>Number of records deleted</returns>
        Task<int> DeleteTranscriptAsync(int transcriptId);

		/// <summary>
		/// Udpate the examination type for several dialogs
		/// </summary>
		/// <param name="examinatonDtoList">Dto list with the information of examination change.</param>
		/// <param name="transcriptId">Transcription Id.</param>
		/// <param name="requestUserId">User request Id.</param>
		/// /// <param name="transcriptId">User Connection Id.</param>
		/// <returns>Number of records changed</returns>
		Task<bool> UpdateExaminationBlockAsync(IEnumerable<SaveExaminationDto> examinatonDtoList, int transcriptId, int requestUserId, string connectionId);

		/// <summary>
		/// Update a single examination tag 
		/// </summary>
		/// <param name="updateExaminationTagsDto">Dto with the information to update a single examination tag </param>
		/// <returns>Number of records changed</returns>
		Task<int> UpdateSingleExaminationTagAsync(SaveSingleExaminationTagDto updateExaminationTagsDto);

		/// <summary>
		/// Update the examination tag massively by Original person Id 
		/// </summary>
		/// <param name="updateExaminationTagsDto">Dto with the information to update examination tags massively by person id</param>
		/// <returns>Number of records changed</returns>
		Task<int> UpdateMassivelyExaminationTagAsync(SaveMassiveExaminationTagDto saveMassiveExaminationTagDto);

		/// <summary>
		/// Lock/unlock a transcript
		/// </summary>
		/// <param name="lockTranscriptDto">Lock Transcript dto</param>
		/// <returns>Number of records changed</returns>
		Task<int> LockTranscriptAsync(LockTranscriptDto lockTranscriptDto);

		/// <summary>
		/// Get the available transcription languages
		/// </summary>
		/// <returns>List of transcription languages</returns>
		Task<IEnumerable<ApiLanguage>> GetAllTranscriptionLanguagesAsync();

		/// <summary>
		/// Request Human Transcription job
		/// </summary>
		/// <param name="transcriptDto">Transcript object to request Human Transcription.</param>
		/// <returns>True if the request was created.</returns>
		Task<bool> CreateHumanTranscriptionAsync(Transcript transcriptDto);
	}
}
