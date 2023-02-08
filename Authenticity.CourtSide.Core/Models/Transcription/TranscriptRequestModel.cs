namespace Authenticity.CourtSide.Core.Models.Transcription
{
	public class TranscriptRequestModel
	{
		public string PathFile { get; set; }
		public string ApiUrl { get; set; }
		public string ApiGuid { get; set; }
		public string ApiLanguage { get; set; }
		public string TranscriptionEngine { get; set; }
	}
}
