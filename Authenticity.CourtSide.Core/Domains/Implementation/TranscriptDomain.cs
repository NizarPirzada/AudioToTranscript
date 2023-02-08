using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class TranscriptDomain : ITranscriptDomain
	{
		private readonly ITranscriptContext _transcriptContext;

		#region [Public]

		public TranscriptDomain(ITranscriptContext transcriptContext)
		{
			_transcriptContext = transcriptContext;
		}

		public async Task<IEnumerable<Transcript>> GetAllTranscriptsByUserAsync(int userId)
		{
			IEnumerable<Transcript> allTranscripts = await _transcriptContext.TranscriptQueryRepository.GetAllTranscriptsByUserAsync(userId);

			foreach (Transcript t in allTranscripts)
			{
				IEnumerable<TranscriptFile> transcriptFiles = await _transcriptContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(t.Id);
				t.LastModifiedOn = (t.LastModifiedOn == DateTime.MinValue) ? t.CreatedOn : t.LastModifiedOn;
				t.TranscriptFile = transcriptFiles.FirstOrDefault();
			}

			return allTranscripts;
		}

		public async Task<Transcript> GetTranscriptByIdAsync(int id)
		{
			var response = await _transcriptContext.TranscriptQueryRepository.GetTranscriptByIdAsync(id);
			return response;
		}

		public async Task<Transcript> CreateSimpleTranscriptAsync(Transcript transcriptDto)
		{
			var transcriptId = await _transcriptContext.TranscriptCommandRepository.CreateTranscriptWithNameAsync(transcriptDto);
			Transcript transcriptCreated = await _transcriptContext.TranscriptQueryRepository.GetTranscriptByIdAsync(transcriptId);

			return transcriptCreated;
		}

		public async Task<Transcript> UpdateTranscriptAsync(Transcript transcriptDto)
		{
			await _transcriptContext.TranscriptCommandRepository.UpdateTranscriptAsync(transcriptDto);

			if (transcriptDto.Court?.CourtId > 0)
			{
				transcriptDto.Court.LastModifiedBy = transcriptDto.LastModifiedBy;
				transcriptDto.Court.TranscriptId = transcriptDto.Id;
				await _transcriptContext.CourtCommandRepository.UpdateCourtAsync(transcriptDto.Court);
			}
			else
			{
				transcriptDto.Court.CreatedBy = transcriptDto.LastModifiedBy;
				transcriptDto.Court.TranscriptId = transcriptDto.Id;
				await _transcriptContext.CourtCommandRepository.CreateCourtAsync(transcriptDto.Court);
			}

			if (transcriptDto.TranscriptType == TranscriptTypeEnum.Deposition)
			{
				if (transcriptDto.TranscriptRecordingInfo.TranscriptRecordingInfoId > 0)
				{
					transcriptDto.TranscriptRecordingInfo.LastModifiedBy = transcriptDto.LastModifiedBy;
					transcriptDto.TranscriptRecordingInfo.TranscriptId = transcriptDto.Id;
					await _transcriptContext.TranscriptRecordingInfoCommandRepository.UpdateTranscriptRecordingInfoAsync(transcriptDto.TranscriptRecordingInfo);
				}
				else
				{
					transcriptDto.TranscriptRecordingInfo.CreatedBy = transcriptDto.LastModifiedBy;
					transcriptDto.TranscriptRecordingInfo.TranscriptId = transcriptDto.Id;
					await _transcriptContext.TranscriptRecordingInfoCommandRepository.CreateTranscriptRecordingInfoAsync(transcriptDto.TranscriptRecordingInfo);
				}
			}

			Transcript transcriptUpdated =
				await _transcriptContext.TranscriptQueryRepository.GetTranscriptByIdAsync(transcriptDto.Id);

			return transcriptUpdated;
		}

		public Task<IEnumerable<TranscriptPerson>> GetAllTranscriptPersonByIdAsync(int transcriptId)
		{
			var response = _transcriptContext.TranscriptPersonQueryRepository.GetAllTranscriptPersonsByTranscriptIdAsync(transcriptId);
			return response;
		}

		public async Task<TranscriptPerson> CreateTranscriptPersonAsync(TranscriptPerson person)
		{
			int newPersonId = await _transcriptContext.TranscriptPersonCommandRepository.CreatePersonAsync(person);

			if (person.AdditionalInfo != null && CheckEmptyPersonAdditionalInformation(person.AdditionalInfo))
			{

				person.AdditionalInfo.PersonAdditionalInformationId = await _transcriptContext.PersonAdditionalInfoCommandRepository.SaveAdditionalInfoAsync(person.AdditionalInfo);
				person.Id = newPersonId;
				await _transcriptContext.TranscriptPersonCommandRepository.UpdatePersonAsync(person);
			}

			TranscriptPerson resultPerson =
				await _transcriptContext.TranscriptPersonQueryRepository.GeTranscriptPersonByIdAsync(newPersonId);

			return resultPerson;
		}

		public async Task<TranscriptPerson> UpdateTranscriptPersonAsync(TranscriptPerson person)
		{
			if (person.AdditionalInfo.PersonAdditionalInformationId != default)
			{
				await _transcriptContext.PersonAdditionalInfoCommandRepository.UpdateAdditionalInfoAsync(person.AdditionalInfo);
			}
			else if (CheckEmptyPersonAdditionalInformation(person.AdditionalInfo))
			{
				person.AdditionalInfo.PersonAdditionalInformationId = await _transcriptContext.PersonAdditionalInfoCommandRepository.SaveAdditionalInfoAsync(person.AdditionalInfo);
			}
			await _transcriptContext.TranscriptPersonCommandRepository.UpdatePersonAsync(person);

			TranscriptPerson resultPerson =
				await _transcriptContext.TranscriptPersonQueryRepository.GeTranscriptPersonByIdAsync(person.Id);

			return resultPerson;
		}

		public async Task<Transcript> SaveTranscriptMediaInformationAsync(TranscriptFile file)
		{
			Transcript transcript = await _transcriptContext.TranscriptQueryRepository.GetTranscriptByIdAsync(file.TranscriptId);

			var prevFiles = await _transcriptContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(transcript.Id);

			if (!prevFiles.Any() && !string.IsNullOrEmpty(file.Path))
			{
				UserModel user = await _transcriptContext.UserQueryRepository.GetUserByIdAsync(file.CreatedBy);

				await _transcriptContext.TranscriptFileCommandRepository.DeleteDuplicatedFileAsync(transcript.Id);

				await _transcriptContext.TranscriptFileCommandRepository.SaveFileAsync(file);

				TranscriptionEngine transcriptionEngine = await _transcriptContext.TranscriptionEngineQueryRepository.GetTranscriptionEngineByIdAsync(user.TranscriptionEngineId);

				TranscriptJob job = new TranscriptJob()
				{
					TranscriptId = transcript.Id,
					CreatedBy = file.CreatedBy,
					Status = Enums.TranscriptJobStatusEnum.Created,
					JobGuid = string.Empty,
					ApiUrl = user.ApiUrl,
					TranscriptionEngine = transcriptionEngine.Code
				};

				await _transcriptContext.TranscriptJobCommandRepository.CreateJobAsync(job);
			}

			return transcript;
		}

		public async Task<IEnumerable<TranscriptFile>> GetTranscriptMediaFileListByIdAsync(int transcriptId)
		{
			IEnumerable<TranscriptFile> allFiles = await _transcriptContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(transcriptId);
			// Remove the FTP path for Web API
			var response = allFiles.Select(f =>
			{
				f.Path = string.Empty;
				return f;
			});
			return response;
		}

		public Task<IEnumerable<TranscriptDialog>> GetTranscriptDialogsByIdAndPaginationAsync(int transcriptId, int page)
		{
			var allFiles = _transcriptContext.TranscriptDialogQueryRepository.GetAllTranscriptDialogByIdAndPaginationAsync(transcriptId, page);
			return allFiles;
		}

		public Task<int> UpdateTranscriptDialogAsync(TranscriptDialog dialog)
		{
			return _transcriptContext.TranscriptDialogCommandRepository.UpdateDialogAsync(dialog);
		}

		public Task<int> UpdateAllSpeakersAsync(ChangeSpeakerDto speakerRequest)
		{
			return _transcriptContext.TranscriptDialogCommandRepository.UpdateAllSpeakersAsync(speakerRequest);
		}

		public async Task<int> DeleteTranscriptPersonAsync(int transcriptPersonId)
		{
			TranscriptPerson person =
				await _transcriptContext.TranscriptPersonQueryRepository
					.GeTranscriptPersonByIdAsync(transcriptPersonId);

			int affectedRows = default;

			if (person.AdditionalInfo.PersonAdditionalInformationId.GetValueOrDefault() != default)
			{
				affectedRows += await _transcriptContext.PersonAdditionalInfoCommandRepository.DeleteAdditionalInfoAsync(
					person.AdditionalInfo.PersonAdditionalInformationId.GetValueOrDefault());
			}

			affectedRows += await _transcriptContext.TranscriptPersonCommandRepository.DeletePersonAsync(person.Id);

			return affectedRows;
		}
		public async Task<TranscriptionStatus> GetActiveTrancriptJobAsync(int transcriptId)
		{
			TranscriptJob activeJob = await _transcriptContext.TranscriptJobQueryRepository.GetActiveJobByTranscriptAsync(transcriptId);

			TranscriptionStatus transcriptionStatus = new TranscriptionStatus()
			{
				TranscriptId = activeJob.TranscriptId,
				Status = activeJob.Status,
				Message = activeJob.Message,
				CreatedOn = activeJob.CreatedOn,
				StartSendingOn = activeJob.StartSendingToApiOn,
				SentOn = activeJob.SentToApiOn,
				CompletedOn = activeJob.CompletedOn,
			};

			return transcriptionStatus;
		}

		public async Task<bool> ResendTranscriptJobAsync(TranscriptJob transcriptJob)
		{
			//Cancel current job in progress
			await _transcriptContext.TranscriptJobCommandRepository.CancelJobsInProgressAsync(transcriptJob.TranscriptId);
			//Create a new one job

			UserModel user = await _transcriptContext.UserQueryRepository.GetUserByIdAsync(transcriptJob.CreatedBy);
			TranscriptionEngine transcriptionEngine = await _transcriptContext.TranscriptionEngineQueryRepository.GetTranscriptionEngineByIdAsync(user.TranscriptionEngineId);

			TranscriptJob job = new TranscriptJob()
			{
				TranscriptId = transcriptJob.TranscriptId,
				CreatedBy = transcriptJob.CreatedBy,
				Status = TranscriptJobStatusEnum.Created,
				JobGuid = string.Empty,
				ApiUrl = user.ApiUrl,
				TranscriptionEngine = transcriptionEngine.Code
			};

			await _transcriptContext.TranscriptJobCommandRepository.CreateJobAsync(job);
			return true;
		}

		public async Task<int> DeleteTranscriptAsync(int transcriptId)
		{
			int rowsAffected = default;

			TranscriptFile transcriptFile =
				(await _transcriptContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(transcriptId)).FirstOrDefault();

			if (transcriptFile != null)
			{
				// Delete File in file server
				IFileProvider fileProvider = await _transcriptContext.FileProviderFactory.CreateAsync();
				await fileProvider.DeleteFileAsync(transcriptFile.Path);

				// Delete file in database
				rowsAffected += await _transcriptContext.TranscriptFileCommandRepository.DeleteAllFilesByTranscriptIdAsync(transcriptId);
			}

			// Delete transcript dialogs
			rowsAffected +=
				await _transcriptContext.TranscriptDialogCommandRepository.DeleteDialogsByTranscriptIdAsync(
					transcriptId);

			// Delete transcript people
			rowsAffected +=
				await _transcriptContext.TranscriptPersonCommandRepository.DeleteAllPeopleByTranscriptIdAsync(
					transcriptId);

			// Delete transcript court
			rowsAffected += await _transcriptContext.CourtCommandRepository.DeleteCourtByTranscriptIdAsync(transcriptId);

			//  Delete transcript jobs
			rowsAffected +=
				await _transcriptContext.TranscriptJobCommandRepository.DeleteAllJobsByTranscriptIdAsync(transcriptId);

			// Delete transcript recording info
			rowsAffected += await _transcriptContext.TranscriptRecordingInfoCommandRepository.DeleteTranscriptRecordingInfoByTranscriptIdAsync(transcriptId);

			// Delete transcript
			rowsAffected += await _transcriptContext.TranscriptCommandRepository.DeleteAsync(transcriptId);

			return rowsAffected;
		}

		public async Task<bool> UpdateExaminationBlockAsync(IEnumerable<SaveExaminationDto> examinatonDtoList, int transcriptId, int requestUserId, string connectionId)
		{
			LockTranscriptDto lockTranscriptDto = new LockTranscriptDto()
			{
				TranscriptId = transcriptId,
				Locked = true,
				LastModifiedBy = requestUserId
			};

			SaveExaminationsAsync(lockTranscriptDto, examinatonDtoList, transcriptId, connectionId);

			return true;
		}

		public Task<int> UpdateSingleExaminationTagAsync(SaveSingleExaminationTagDto saveExaminationTagsDto)
		{

			TranscriptDialog transcriptDialog = new TranscriptDialog()
			{
				TranscriptId = saveExaminationTagsDto.TranscriptId,
				Id = saveExaminationTagsDto.DialogId,
				ExaminationTag = saveExaminationTagsDto.ExaminationTag
			};

			return _transcriptContext.TranscriptDialogCommandRepository.UpdateSingleExaminationTagAsync(transcriptDialog);
		}

		public Task<int> UpdateMassivelyExaminationTagAsync(SaveMassiveExaminationTagDto saveMassiveExaminationTagDto)
		{
			TranscriptDialog from = new TranscriptDialog();
			TranscriptDialog to = new TranscriptDialog();
			from.TranscriptId = to.TranscriptId = saveMassiveExaminationTagDto.TranscriptId;
			from.ExaminationTag = to.ExaminationTag = saveMassiveExaminationTagDto.ExaminationTag;
			from.Id = saveMassiveExaminationTagDto.StartExaminationId;
			from.OriginalSpeakerId = saveMassiveExaminationTagDto.OriginalSpeakerId;
			to.Id = saveMassiveExaminationTagDto.CloseExaminationId;

			return _transcriptContext.TranscriptDialogCommandRepository.UpdateMassivelyExaminationTagAsync(from, to);
		}

		public Task<int> LockTranscriptAsync(LockTranscriptDto lockTranscriptDto)
		{
			return _transcriptContext.TranscriptCommandRepository.LockTranscriptAsync(lockTranscriptDto);
		}

		public Task<IEnumerable<ApiLanguage>> GetAllTranscriptionLanguagesAsync()
		{
			return _transcriptContext.LanguageQueryRepository.GetAllLanguagesByTypeAsync(LanguageTypeEnum.Transcription);
		}

		public async Task<bool> CreateHumanTranscriptionAsync(Transcript transcriptDto)
		{
			bool requestStatus = false;
			Transcript transcript = await _transcriptContext.TranscriptQueryRepository.GetTranscriptByIdAsync(transcriptDto.Id);
			if (transcript.HumanTranscriptionStatus != TranscriptHumanTranscriptionStatusEnum.Completed &&
				transcript.HumanTranscriptionStatus != TranscriptHumanTranscriptionStatusEnum.Requested)
			{
				// In a future implementation, the request must be coded.
				// The request for human transcription is not created becasue this feature is only for demo purposes.
				requestStatus = true;
				transcript.HumanTranscriptionStatus = TranscriptHumanTranscriptionStatusEnum.Requested;
				await _transcriptContext.TranscriptCommandRepository.UpdateTranscriptAsync(transcript);
			}
			return requestStatus;
		}

		#endregion

		#region [Private]

		private static bool CheckEmptyPersonAdditionalInformation(PersonAdditionalInformation additionalInfo)
		{
			return !string.IsNullOrWhiteSpace(additionalInfo.Address) ||
				!string.IsNullOrWhiteSpace(additionalInfo.BarNumber) ||
				!string.IsNullOrWhiteSpace(additionalInfo.Telephone) ||
				!string.IsNullOrWhiteSpace(additionalInfo.Title);
		}

		private async Task SaveExaminationsAsync(LockTranscriptDto lockTranscriptDto, IEnumerable<SaveExaminationDto> examinatonDtoList, int transcriptId, string connectionId)
		{
			try
			{
				await _transcriptContext.TranscriptCommandRepository.LockTranscriptAsync(lockTranscriptDto);
				
				Parallel.ForEach(examinatonDtoList, async (item) =>
				{
					TranscriptDialog transcriptDialog = new TranscriptDialog()
					{
						TranscriptId = transcriptId,
						Id = item.DialogId,
						ExaminationType = item.ExaminationType,
						ExaminationTag = item.ExaminationTag
					};

					await _transcriptContext.TranscriptDialogCommandRepository.UpdateSingleExaminationAsync(transcriptDialog);

				});

				lockTranscriptDto.Locked = false;
				await _transcriptContext.TranscriptCommandRepository.LockTranscriptAsync(lockTranscriptDto);

				await _transcriptContext.TranscriptionNotificationHub.NotifyExaminationSavingStatusAsync(connectionId, "Changes are saving");

			}
			catch (Exception ex)
			{
				lockTranscriptDto.Locked = false;
				await _transcriptContext.TranscriptCommandRepository.LockTranscriptAsync(lockTranscriptDto);

				Parallel.ForEach(examinatonDtoList, async (item) =>
				{
					TranscriptDialog transcriptDialog = new TranscriptDialog()
					{
						TranscriptId = transcriptId,
						Id = item.DialogId,
						ExaminationType = TranscriptExaminationEnum.None,
						ExaminationTag = TranscriptExaminationTagEnum.None,
					};

					await _transcriptContext.TranscriptDialogCommandRepository.UpdateSingleExaminationAsync(transcriptDialog);
				});

				await _transcriptContext.TranscriptionNotificationHub.NotifyExaminationSavingStatusAsync(connectionId, "Error saving examinations");
			}
		}

		#endregion
	}
}
