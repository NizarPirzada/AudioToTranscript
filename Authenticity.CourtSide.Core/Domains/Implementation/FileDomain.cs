using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class FileDomain : IFileDomain
	{
		public FileDomain(IFileContext fileContext, IFileProviderFactory fileProviderFactory, ILogger<FileDomain> logger, ITranscriptionEngineQueryRepository transcriptionEngineQueryRepository)
		{
			FileContext = fileContext;
			FileProviderFactory = fileProviderFactory;
			Logger = logger;
			TranscriptionEngineQueryRepository = transcriptionEngineQueryRepository;
		}

		private IFileContext FileContext { get; }
		public IFileProviderFactory FileProviderFactory { get; }
		private ILogger<FileDomain> Logger { get; }
		public ITranscriptionEngineQueryRepository TranscriptionEngineQueryRepository { get; }

		public Task<string> SaveFileTempLocationAsync(TranscriptFile file)
		{
			return FileContext.LocalFileHelper.SaveFileAsync(file.File.FileName, file.File);
		}
		public Task<bool> DeleteFileTempLocationAsync(string filePath)
		{
			return FileContext.LocalFileHelper.DeleteFileAsync(filePath);
		}

		public async Task<string> SaveFileToFTPAsync(int transcriptId, decimal size, string fileName)
		{
			string destinationFileName = Guid.NewGuid().ToString();
			IFileProvider fileProvider = await FileProviderFactory.CreateAsync();
			await fileProvider.SaveFileAsync(fileName, destinationFileName);

			Transcript transcript = await FileContext.TranscriptQueryRepository.GetTranscriptByIdAsync(transcriptId);
			var prevFiles = await FileContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(transcript.Id);
			if (!prevFiles.Any())
			{
				UserModel user = await FileContext.UserQueryRepository.GetUserByIdAsync(transcript.CreatedBy);
				await FileContext.TranscriptFileCommandRepository.DeleteDuplicatedFileAsync(transcript.Id);

				TranscriptFile file = new TranscriptFile()
				{
					TranscriptId = transcript.Id,
					Name = Path.GetFileName(fileName),
					Path = destinationFileName
				};
				await FileContext.TranscriptFileCommandRepository.SaveFileAsync(file);

				TranscriptionEngine transcriptionEngine = await TranscriptionEngineQueryRepository.GetTranscriptionEngineByIdAsync(user.TranscriptionEngineId);

				TranscriptJob job = new TranscriptJob()
				{
					TranscriptId = transcript.Id,
					CreatedBy = user.Id,
					Status = Enums.TranscriptJobStatusEnum.Created,
					JobGuid = string.Empty,
					ApiUrl = user.ApiUrl,
					TranscriptionEngine = transcriptionEngine.Code
				};

				await FileContext.TranscriptJobCommandRepository.CreateJobAsync(job);

				transcript.MediaFileSize = size / 1024;
				transcript.Status = Enums.TranscriptStatusEnum.Processing;
				transcript.Step = 3;
				await FileContext.TranscriptCommandRepository.UpdateTranscriptAsync(transcript);
			}
			return destinationFileName;
		}

		public async Task<FileStream> GetAudioFileAsync(int transcriptId)
		{
			string tempFolderName = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "CourtSide");

			if (Int32.TryParse(FileContext.Configuration["CleanTempFolderInHours"], out int previousHours))
			{
				FileContext.LocalFileHelper.CleanTempFolder(tempFolderName, previousHours);
			}

			IEnumerable<TranscriptFile> transcriptFiles = await FileContext.TranscriptFileQueryRepository.GetAllTranscriptFileByIdAsync(transcriptId);

			if (transcriptFiles.Any())
			{
				if (!Directory.Exists(tempFolderName))
				{
					Directory.CreateDirectory(tempFolderName);
				}

				string transcriptPath = transcriptFiles.FirstOrDefault().Path;
				string fileName = Path.GetFileName(transcriptPath);
				string transcriptTempFile = Path.Combine(tempFolderName, fileName);
				FileStream fileStream;

				if (!File.Exists(transcriptTempFile))
				{
					IFileProvider fileProvider = await FileProviderFactory.CreateAsync();
					FileStream localFile = await fileProvider.GetFileStreamAsync(transcriptPath, transcriptTempFile);
					fileStream = File.OpenRead(localFile.Name);
				}
				else
				{
					fileStream = File.OpenRead(transcriptTempFile);
				}

				return fileStream;
			}
			else
			{
				throw new FileNotFoundException("Transcription file not found");
			}
		}
	}
}
