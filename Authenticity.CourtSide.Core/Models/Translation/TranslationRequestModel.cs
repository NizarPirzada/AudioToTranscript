namespace Authenticity.CourtSide.Core.Models.Transcription
{
	public class TranslationRequestModel
	{
		public string PathFile { get; set; }
		public string ApiUrl { get; set; }
		public string ApiGuid { get; set; }
		public string SourceLanguage { get; set; }
		public string OutputLanguage { get; set; }
	}
}
