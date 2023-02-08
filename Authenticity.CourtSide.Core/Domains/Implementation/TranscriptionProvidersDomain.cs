using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Models.Transcription;
using Newtonsoft.Json.Linq;
using NSerio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class TranscriptionProvidersDomain : ITranscriptionProvidersDomain
	{
		private const string ERROR_INVALID_USER_CREDENTIALS = "User API credentials cannot be empty";
		private const string ERROR_ENGINE_JOB = "The Transcritpion Engine job has failed. Please try to retry the transcription.";

		private readonly ITranscriptionProvidersDomainContext _transcriptionProvidersContext;

		public TranscriptionProvidersDomain(ITranscriptionProvidersDomainContext transcriptionProvidersDomainContext)
		{
			_transcriptionProvidersContext = transcriptionProvidersDomainContext;
		}

		public async Task<int> SendNewJobsToTranscriptionEngineAsync()
		{
			int totalJobsProcessed = 0;

			var newJobs = await _transcriptionProvidersContext.TranscriptJobQueryRepository.GetBatchOfJobsByStatusAsync(Enums.TranscriptJobStatusEnum.Created);

			if (newJobs.HasValues())
			{
				foreach (var job in newJobs)
				{
					var transcriptFiles = await _transcriptionProvidersContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(job.TranscriptId);
					var transcriptFile = transcriptFiles.FirstOrDefault();
					var localFilePath = string.Empty;

					job.StartSendingToApiOn = DateTime.UtcNow;
					await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);

					try
					{
						//Getfile from 
						IFileProvider fileProvider = await _transcriptionProvidersContext.FileProviderFactory.CreateAsync();
						localFilePath = await fileProvider.GetFileToLocalTempDirectoryAsync(transcriptFile.Path, transcriptFile.Name);
						UserModel jobUser =
							await _transcriptionProvidersContext.UserQueryRepository.GetUserByIdAsync(job.CreatedBy);

						Transcript transcript = await _transcriptionProvidersContext.TranscriptQueryRepository.GetTranscriptByIdAsync(job.TranscriptId);
						ApiLanguage transcriptionLanguage = await _transcriptionProvidersContext.LanguageQueryRepository.GetLanguageByIdAsync(transcript.ApiLanguageId);

						TranscriptRequestModel transcriptRequest = new TranscriptRequestModel()
						{
							PathFile = localFilePath,
							ApiUrl = string.IsNullOrWhiteSpace(job.ApiUrl) ? jobUser.ApiUrl : job.ApiUrl,
							ApiGuid = jobUser.ApiGuid,
							ApiLanguage = transcriptionLanguage.ApiCode,
							TranscriptionEngine = job.TranscriptionEngine
						};

						if (string.IsNullOrWhiteSpace(transcriptRequest.ApiGuid) ||
							string.IsNullOrWhiteSpace(transcriptRequest.ApiUrl))
						{
							throw new ApplicationException(ERROR_INVALID_USER_CREDENTIALS);
						}

						string jobGuid = await _transcriptionProvidersContext.TranscriptionEngine.CreateTranscriptRequestAsync(transcriptRequest);
						if (jobGuid != null)
						{
							job.JobGuid = jobGuid;
							job.SentToApiOn = DateTime.UtcNow;
							job.Status = Enums.TranscriptJobStatusEnum.SentToAuthenticity;
						}
						else
						{
							job.JobGuid = string.Empty;
							job.Status = Enums.TranscriptJobStatusEnum.Created;
						}

						await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
						totalJobsProcessed++;
					}
					catch (Exception ex)
					{
						// Fails silently
						job.Status = Enums.TranscriptJobStatusEnum.Error;
						job.CompletedOn = DateTime.UtcNow;
						job.Message = ERROR_ENGINE_JOB;
						await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
					}
					finally
					{
						if (File.Exists(localFilePath))
						{
							File.Delete(localFilePath);
						}
					}
				}
			}

			return totalJobsProcessed;
		}

		public async Task<int> CheckPendingJobsFromTranscriptionEngineAsync()
		{
			int totalJobsProcessed = 0;

			var newJobs = await _transcriptionProvidersContext.TranscriptJobQueryRepository.GetBatchOfJobsByStatusAsync(Enums.TranscriptJobStatusEnum.SentToAuthenticity);

			if (newJobs.HasValues())
			{
				foreach (var job in newJobs)
				{
					JObject engineResponse = null;
					try
					{
						if (string.IsNullOrWhiteSpace(job.ApiUrl))
						{
							UserModel jobUser = await _transcriptionProvidersContext.UserQueryRepository.GetUserByIdAsync(job.CreatedBy);
							job.ApiUrl = jobUser.ApiUrl;
							job.Status = Enums.TranscriptJobStatusEnum.Processing;
							await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
						}

						string jobResult =
							await _transcriptionProvidersContext.TranscriptionEngine.CheckTranscriptResponseAsync(
								job.ApiUrl, job.JobGuid);

						if (jobResult != null)
						{
							engineResponse = JObject.Parse(jobResult);

							if (engineResponse["status"].Value<string>() == "SUCCESS")
							{
								IList<JToken> results = engineResponse["task_result"]["transcripts"].Children().ToList();
								List<TranscriptPerson> speakers = new List<TranscriptPerson>();

								foreach (JToken result in results)
								{
									AuthenticityTranscriptConversationResponse chunk = result.ToObject<AuthenticityTranscriptConversationResponse>();
									TranscriptDialog newDialog = new TranscriptDialog()
									{
										TranscriptId = job.TranscriptId,
										StartTime = chunk.StartTime,
										Duration = chunk.Duration,
										Transcription = chunk.Transcription
									};
									int speakerId = 0;
									if (!speakers.Where(s => s.FirstName.Compare(chunk.Speaker)).Any())
									{
										speakerId = speakers.Count + 1;
										speakers.Add(new TranscriptPerson()
										{
											Id = speakerId,
											FirstName = chunk.Speaker
										});
									}
									else
									{
										speakerId = speakers.Where(s => s.FirstName.Compare(chunk.Speaker)).FirstOrDefault().Id;
									}
									newDialog.OriginalSpeakerId = speakerId;
									newDialog.OriginalSpeakerName = chunk.Speaker;
									await _transcriptionProvidersContext.TranscriptDialogCommandRepository.CreateDialogAsync(newDialog);
								}
								await _transcriptionProvidersContext.TranscriptCommandRepository.UpdateTranscriptTextAsync(job.TranscriptId, jobResult);

								job.Status = Enums.TranscriptJobStatusEnum.Completed;
								job.CompletedOn = DateTime.UtcNow;
							}
							else
							{
								job.Status = Enums.TranscriptJobStatusEnum.SentToAuthenticity;
							}

							await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
						}

						totalJobsProcessed++;
					}
					catch (ApplicationException)
					{
						string engineResponseMessage = engineResponse["message"].Value<string>();
						job.Status = Enums.TranscriptJobStatusEnum.Error;
						job.CompletedOn = DateTime.UtcNow;
						job.Message = string.IsNullOrWhiteSpace(engineResponseMessage) ? ERROR_ENGINE_JOB : engineResponseMessage;
						await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
					}
					catch (Exception)
					{
						job.Status = Enums.TranscriptJobStatusEnum.SentToAuthenticity;
						await _transcriptionProvidersContext.TranscriptJobCommandRepository.UpdateJobAsync(job);
					}
				}
			}

			return totalJobsProcessed;
		}


	}
}
