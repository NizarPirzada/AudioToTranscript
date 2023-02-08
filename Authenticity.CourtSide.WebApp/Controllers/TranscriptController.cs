using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.WebApp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class TranscriptController : ApiControllerBase
	{
		public TranscriptController(ITranscriptDomain transcriptDomain, ILogger<TranscriptController> logger)
		{
			TranscriptDomain = transcriptDomain;
			Logger = logger;
		}
		private ITranscriptDomain TranscriptDomain { get; }
		private ILogger<TranscriptController> Logger { get; }

		[HttpGet]
		[Route("GetAll")]
		public async Task<IActionResult> GetAllTranscriptsByUserAsync()
		{
			try
			{
				var allTranscripts = await TranscriptDomain.GetAllTranscriptsByUserAsync(UserId);

				BaseResponse<IEnumerable<Transcript>> response = new BaseResponse<IEnumerable<Transcript>>()
				{
					Data = allTranscripts,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllTranscriptsByUserAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetById/{id}")]
		public async Task<IActionResult> GeTranscriptsByIdAsync(int id)
		{
			try
			{
				var transcript = await TranscriptDomain.GetTranscriptByIdAsync(id);

				BaseResponse<Transcript> response = new BaseResponse<Transcript>()
				{
					Data = transcript,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GeTranscriptsByIdAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("SaveSimple")]
		public async Task<IActionResult> CreateTranscriptWithNameAsync(Transcript transcript)
		{
			try
			{
				Transcript model = new Transcript()
				{
					Name = transcript.Name,
					Status = Core.Enums.TranscriptStatusEnum.Created,
					MediaFileSize = 0,
					CreatedBy = UserId
				};
				Transcript transcriptCreated = await TranscriptDomain.CreateSimpleTranscriptAsync(model);

				BaseResponse<Transcript> response = new BaseResponse<Transcript>()
				{
					Data = transcriptCreated,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CreateTranscriptWithNameAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("Edit")]
		public async Task<IActionResult> SaveBasicInfoTranscriptAsync(Transcript transcript)
		{
			try
			{
				transcript.LastModifiedBy = UserId;
				Transcript transcriptCreated = await TranscriptDomain.UpdateTranscriptAsync(transcript);

				BaseResponse<Transcript> response = new BaseResponse<Transcript>()
				{
					Data = transcriptCreated,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SaveBasicInfoTranscriptAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("UpdateRecordingDate")]
		public async Task<IActionResult> SaveTranscriptRecordingDateAsync(Transcript transcript)
		{
			try
			{
				Transcript currentTranscript = await TranscriptDomain.GetTranscriptByIdAsync(transcript.Id);
				currentTranscript.RecordDate = transcript.RecordDate;
				currentTranscript.LastModifiedBy = UserId;
				Transcript transcriptCreated = await TranscriptDomain.UpdateTranscriptAsync(currentTranscript);

				BaseResponse<Transcript> response = new BaseResponse<Transcript>()
				{
					Data = transcriptCreated,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SaveBasicInfoTranscriptAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetPersons/{transcriptId}")]
		public async Task<IActionResult> GetAllTranscriptPersonByIdAsync(int transcriptId)
		{
			try
			{
				var allPersons = await TranscriptDomain.GetAllTranscriptPersonByIdAsync(transcriptId);

				BaseResponse<IEnumerable<TranscriptPerson>> response = new BaseResponse<IEnumerable<TranscriptPerson>>()
				{
					Data = allPersons,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllTranscriptPersonByIdAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("SavePerson")]
		public async Task<IActionResult> SavePersonAsync(TranscriptPerson person)
		{
			try
			{
				TranscriptPerson result;
				person.CreatedBy = UserId;
				if (person.Id == 0)
				{
					result = await TranscriptDomain.CreateTranscriptPersonAsync(person);
				}
				else
				{
					result = await TranscriptDomain.UpdateTranscriptPersonAsync(person);
				}

				BaseResponse<TranscriptPerson> response = new BaseResponse<TranscriptPerson>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SavePersonAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("SaveMediaInfo")]
		public async Task<IActionResult> SaveTranscriptMediaInformationAsync(TranscriptFile file)
		{
			try
			{
				file.CreatedBy = UserId;
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid transcript data." });
				}

				Transcript transcriptCreated = await TranscriptDomain.SaveTranscriptMediaInformationAsync(file);

				BaseResponse<Transcript> response = new BaseResponse<Transcript>()
				{
					Data = transcriptCreated,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SaveTranscriptMediaInformationAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetMediaFileList/{transcriptId}")]
		public async Task<IActionResult> GetTranscriptMediaFileListByIdAsync(int transcriptId)
		{
			try
			{
				var allFiles = await TranscriptDomain.GetTranscriptMediaFileListByIdAsync(transcriptId);

				BaseResponse<IEnumerable<TranscriptFile>> response = new BaseResponse<IEnumerable<TranscriptFile>>()
				{
					Data = allFiles,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetTranscriptMediaFileListByIdAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetDialogs/{transcriptId}/{page}")]
		public async Task<IActionResult> GetAllTranscriptDialogByIdAsync(int transcriptId, int page)
		{
			try
			{
				var allDialogs = await TranscriptDomain.GetTranscriptDialogsByIdAndPaginationAsync(transcriptId, page);

				BaseResponse<IEnumerable<TranscriptDialog>> response = new BaseResponse<IEnumerable<TranscriptDialog>>()
				{
					Data = allDialogs,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllTranscriptDialogByIdAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("EditTranscriptDialog")]
		public async Task<IActionResult> UpdateTranscripDialogAsync(TranscriptDialog dialog)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid transcript dialog data." });
				}

				dialog.LastModifiedBy = UserId;

				var result = await TranscriptDomain.UpdateTranscriptDialogAsync(dialog);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateTranscripDialogAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("SaveExaminations/{transcriptId}")]
		public async Task<IActionResult> SaveExaminationsAsync(int transcriptId, IEnumerable<SaveExaminationDto> examinatonDtoList)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid transcript dialog data." });
				}

				int requestUserId = UserId;
				string connectionId = GetUserToken();
				var result = await TranscriptDomain.UpdateExaminationBlockAsync(examinatonDtoList, transcriptId, requestUserId, connectionId);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SaveExaminationsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("EditAllSpeakers")]
		public async Task<IActionResult> UpdateSpeakerInAllDialogsAsync(ChangeSpeakerDto speakerRequest)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid speakers data." });
				}

				var count = await TranscriptDomain.UpdateAllSpeakersAsync(speakerRequest);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = count,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateSpeakerInAllDialogsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpDelete]
		[Route("DeleteTranscriptPerson/{transcriptPersonId}")]
		public async Task<IActionResult> DeleteTranscriptPersonAsync(int transcriptPersonId)
		{
			try
			{
				int affectedRows = await TranscriptDomain.DeleteTranscriptPersonAsync(transcriptPersonId);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = affectedRows,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "DeleteTranscriptPersonAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetJobStatus/{transcriptId}")]
		public async Task<IActionResult> GetJobStatusAsync(int transcriptId)
		{
			try
			{
				TranscriptionStatus currentJob = await TranscriptDomain.GetActiveTrancriptJobAsync(transcriptId);

				BaseResponse<TranscriptionStatus> response = new BaseResponse<TranscriptionStatus>()
				{
					Data = currentJob,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetJobStatusAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("ResendTranscript")]
		public async Task<IActionResult> ResendTranscriptJobAsync(TranscriptJob job)
		{
			try
			{
				job.CreatedBy = UserId;

				var count = await TranscriptDomain.ResendTranscriptJobAsync(job);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Data = count,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "ResendTranscriptJobAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpDelete]
		[Route("Delete/{transcriptId}")]
		public async Task<IActionResult> DeleteTranscriptAsync(int transcriptId)
		{
			try
			{
				int affectedRows = await TranscriptDomain.DeleteTranscriptAsync(transcriptId);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = affectedRows,
					Success = true
				};

				return Ok(response);

			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "DeleteTranscriptAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("UpdateSingleExaminationTag")]
		public async Task<IActionResult> UpdateSingleExaminationTagAsync(SaveSingleExaminationTagDto saveExaminationTag)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid transcript examination tags data." });
				}

				var result = await TranscriptDomain.UpdateSingleExaminationTagAsync(saveExaminationTag);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateSingleExaminationTagAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("UpdateMassivelyExaminationTags")]
		public async Task<IActionResult> UpdateMassivelyExaminationTagsAsync(SaveMassiveExaminationTagDto examinationTagDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid transcript examination tags data." });
				}

				var result = await TranscriptDomain.UpdateMassivelyExaminationTagAsync(examinationTagDto);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateMassivelyExaminationTagsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("LockTranscript")]
		public async Task<IActionResult> LockTranscriptAsync(LockTranscriptDto lockTranscriptDto)
		{
			try
			{

				lockTranscriptDto.LastModifiedBy = UserId;

				var result = await TranscriptDomain.LockTranscriptAsync(lockTranscriptDto);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateMassivelyExaminationTagsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetAllTranscriptionLanguages")]
		public async Task<IActionResult> GetAllTranscriptionLanguagesAsync()
		{
			try
			{
				var result = await TranscriptDomain.GetAllTranscriptionLanguagesAsync();

				BaseResponse<IEnumerable<ApiLanguage>> response = new BaseResponse<IEnumerable<ApiLanguage>>()
				{
					Data = result,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllTranscriptionLanguagesAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("RequestHumanTranscription")]
		public async Task<IActionResult> RequestHumanTranscriptionAsync(Transcript transcript)
		{
			try
			{
				transcript.LastModifiedBy = UserId;
				bool requestStatus = await TranscriptDomain.CreateHumanTranscriptionAsync(transcript);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Data = requestStatus,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "RequestHumanTranscriptionAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}


		#region [PRIVATE]

		private string GetUserToken()
		{
			string bearer = Request.Headers["Authorization"];
			bearer = bearer.Replace("Bearer ", "");

			return bearer;
		}

		#endregion
	}
}
