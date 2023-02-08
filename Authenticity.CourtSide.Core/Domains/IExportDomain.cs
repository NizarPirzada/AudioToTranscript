using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
	public interface IExportDomain : IDomain
    {
		/// <summary>
		/// Get the transcript dialogs to display in formatted preview.
		/// </summary>
		/// <param name="transcriptId">Transcript id.</param>
		/// <returns>Array with dialogs formated with max forty-eight characters per row.</returns>
		Task<string[]> GetTranscriptPreviewLinesAsync(int transcriptId);

		/// <summary>
		/// Export the transcript file to Word format.
		/// </summary>
		/// <param name="exportTranscript">Settings to export the transcript</param>
		/// <returns>File in memory stream.</returns>
		Task<FileStream> ExportTranscriptToWordFormatAsync(ExportTranscriptDto exportTranscript);

		/// <summary>
		/// Translate and Export the transcript file to Word format.
		/// </summary>
		/// <param name="exportTranscript">Settings to export the transcript</param>
		/// <returns>File in memory stream.</returns>
		Task<FileStream> TranslateAndExportTranscriptToWordFormatAsync(ExportTranslateTranscriptDto exportTranscript);

		/// <summary>
		/// Get the available translation languages
		/// </summary>
		/// <returns>List of translation languages</returns>
		Task<IEnumerable<ApiLanguage>> GetAllTranslationLanguagesAsync();
	}
}
