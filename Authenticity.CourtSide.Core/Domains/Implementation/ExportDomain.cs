using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models.Transcription;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
    public class ExportDomain : IExportDomain
    {
        private readonly IExportContext _exportContext;
        private const int MAX_CHARACTER_BY_LINE = 48;
        private const string PROBABLE_CAUSE_HEARING_WORD_TEMPLATE_NAME = "Word-Format-1";
        private const string DEPOSITION_WORD_TEMPLATE_NAME = "Word-Format-Deposition";

        
        #region [Public]

        public ExportDomain(IExportContext exportContext)
        {
            _exportContext = exportContext;
        }

        public Task<string[]> GetTranscriptPreviewLinesAsync(int transcriptId)
        {
            return GetFormatedDialogAsync(transcriptId);
        }

        public Task<IEnumerable<ApiLanguage>> GetAllTranslationLanguagesAsync()
		{
            return _exportContext.LanguageQueryRepository.GetAllLanguagesByTypeAsync(LanguageTypeEnum.Translation);
        }

        public async Task<FileStream> ExportTranscriptToWordFormatAsync(ExportTranscriptDto exportTranscript)
        {
            Transcript transcript = await _exportContext.TranscriptQueryRepository.GetTranscriptByIdAsync(exportTranscript.TranscriptId);
            IEnumerable<TranscriptPerson> transcriptPersons = await _exportContext.TranscriptPersonQueryRepository.GetAllTranscriptPersonsByTranscriptIdAsync(transcript.Id);
            IEnumerable<TranscriptDialog> transcriptDialogs = await _exportContext.TranscriptDialogQueryRepository.GetAllTranscriptDialogByIdAsync(transcript.Id);

            LegalTranscriptHelper.SetTranscriptionWithSpeakerName(transcriptPersons, transcriptDialogs);
            FileExportFormat fileFormat;

            string formatName = String.Empty;

            switch (transcript.TranscriptType)
            {
                case TranscriptTypeEnum.ProbableCauseHearing:
	                formatName = PROBABLE_CAUSE_HEARING_WORD_TEMPLATE_NAME;
                    break;
                case TranscriptTypeEnum.Deposition:
	                formatName = DEPOSITION_WORD_TEMPLATE_NAME;
	                break;
            }
            
            if (exportTranscript.FormatId == 0)
            {
                fileFormat = await _exportContext.FileExportFormatQueryRepository.GetFormatByNameAndTypeAsync(formatName,"DOC");
            }
            else
            {
                fileFormat = await _exportContext.FileExportFormatQueryRepository.GetExportFormatByIdAsync(exportTranscript.FormatId);
            }

            IFileProvider fileProvider = await _exportContext.FileProviderFactory.CreateAsync();
            string formatFullPath = await fileProvider.GetFileToLocalTempDirectoryAsync(fileFormat.Path, $"{transcript.Id}-{Guid.NewGuid().ToString()}.docx");
            string documentPath = _exportContext.FileExporter.CreateExportDocument(formatFullPath, transcript, transcriptPersons, transcriptDialogs, exportTranscript.Offset);

            FileStream fileStream;
            fileStream = File.OpenRead(documentPath);

            return fileStream;
        }

        public async Task<FileStream> TranslateAndExportTranscriptToWordFormatAsync(ExportTranslateTranscriptDto exportTranscript)
        {
            FileStream fileStream = await ExportTranscriptToWordFormatAsync(new ExportTranscriptDto() { 
                TranscriptId = exportTranscript.TranscriptId,
                Offset = exportTranscript.Offset,
                FormatId = exportTranscript.FormatId
            });
            string temporalFileName = $"{Guid.NewGuid().ToString()}.docx";
            string temporalFilePath = await _exportContext.LocalFileHelper.SaveFileStreamAsync(fileStream, temporalFileName);

            // Check Transcription language
            Transcript transcript = await _exportContext.TranscriptQueryRepository.GetTranscriptByIdAsync(exportTranscript.TranscriptId);
            ApiLanguage transcriptApiLanguage = await _exportContext.LanguageQueryRepository.GetLanguageByIdAsync(transcript.ApiLanguageId);
            ApiLanguage translateApiLanguage = await _exportContext.LanguageQueryRepository.GetLanguageByIdAsync(exportTranscript.ApiLanguageId);

			if (transcriptApiLanguage.GenericCode.Equals(translateApiLanguage.GenericCode))
			{
                throw new ArgumentException("Transcription is on the same language");
			}

            UserModel user = await _exportContext.UserQueryRepository.GetUserByIdAsync(exportTranscript.UserId);
            TranslationRequestModel translationRequest = new TranslationRequestModel() { 
                ApiUrl = user.ApiUrl,
                ApiGuid = user.ApiGuid,
                PathFile = temporalFilePath,
				SourceLanguage = transcriptApiLanguage.GenericCode,
				OutputLanguage = translateApiLanguage.ApiCode
            };
            string taskId = await _exportContext.TranslationEngine.CreateTranslationRequestAsync(translationRequest);
            bool translationJobCompleted = false;
            DateTime startTime = DateTime.Now;
            temporalFilePath = string.Empty;
            while (!translationJobCompleted && (DateTime.Now < startTime.AddMinutes(10))) {
                await Task.Delay(2000);
                var translationResponse = await _exportContext.TranslationEngine.CheckTranslationResponseAsync(user.ApiUrl, taskId);
                if (translationResponse != null) {
                    translationJobCompleted = true;
                    temporalFilePath = await _exportContext.LocalFileHelper.SaveStreamAsync(translationResponse, Guid.NewGuid().ToString());
                }
            }
            if (string.IsNullOrEmpty(temporalFilePath)) 
            {
                throw new Exception("The Translation task took more time than expected. Please try again.");
            }
            FileStream translatedFileStream;
            translatedFileStream = File.OpenRead(temporalFilePath);

            return translatedFileStream;
        }

        #endregion

        #region [Private]

        private async Task<string[]> GetFormatedDialogAsync(int transcriptId)
        {
            IEnumerable<TranscriptDialog> dialogs = await _exportContext.TranscriptDialogQueryRepository.GetAllTranscriptDialogByIdAsync(transcriptId);
            List<string> formattedDialog = new List<string>();

            foreach (var dialog in dialogs)
            {
                string textFormatted = $"{ await GetSpeakerName(dialog, transcriptId)}: { dialog.Transcription}";
                textFormatted = FormatToAllowedLines(textFormatted, MAX_CHARACTER_BY_LINE);
                formattedDialog.AddRange(textFormatted.Split(new[] { "\n" }, StringSplitOptions.None));
            }
            return formattedDialog.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
        private int GetPreviousSpace(string text, int startIndex, int fromPosition)
        {
            var spacePosition = fromPosition;
            if (text.Length >= spacePosition && startIndex + fromPosition < text.Length)
            {
                for (var i = startIndex + fromPosition; i > 0; i--)
                {
                    if (Char.IsWhiteSpace(text[i]))
                    {
                        break;
                    }
                    spacePosition--;
                }
            }
            else
            {
                spacePosition = text.Length - startIndex;
            }
            return spacePosition;
        }

        private async Task<string> GetSpeakerName(TranscriptDialog dialog, int transcriptId)
        {
            IEnumerable<TranscriptPerson> persons = await _exportContext.TranscriptPersonQueryRepository.GetAllTranscriptPersonsByTranscriptIdAsync(transcriptId);

            string speakerName = $"Speaker {dialog.OriginalSpeakerId}";
            if (dialog.PersonId != 0)
            {
                var personSelected = persons.Where(s => s.Id == dialog.PersonId).FirstOrDefault();

                if (personSelected != null)
                {
                    speakerName = $"{personSelected.FirstName} { personSelected.LastName}";
                }
            }
            return $"\t{ speakerName}";
        }

        private string FormatToAllowedLines(string dialogText, int maxCharacter)
        {
            int startIndex = 0;
            string newText = "";
            while (startIndex < dialogText.Length - 1)
            {
                var endIndex = startIndex + maxCharacter;
                endIndex = endIndex > dialogText.Length ? dialogText.Length : endIndex;
                var lastSpace = GetPreviousSpace(dialogText, startIndex, maxCharacter);
                
                lastSpace = lastSpace <= 0 ? startIndex : lastSpace;
                if (endIndex - lastSpace >= maxCharacter)
                {
                    lastSpace = maxCharacter;
                }

                if (dialogText.Length >= startIndex + lastSpace)
                {
                    var line = dialogText.Substring(startIndex, lastSpace);
                    newText += $"{ line}\n";
                    startIndex += lastSpace;
                }
                else
                {
                    var line = dialogText.Substring(startIndex);
                    newText += $"{ line}\n";
                    startIndex = dialogText.Length;
                }
            }
            return newText;
        }

        #endregion
    }
}