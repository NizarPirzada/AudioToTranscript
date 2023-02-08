using Authenticity.CourtSide.Core.Helpers;
using System.IO;
using System.Net.Http;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
	public class AuthenticityTranscriptEngineRequest : TranscriptEngineParameter
    {
        public Stream file { get; set; }
        public string engine { get; set; }
        public string language { get; set; }
        public string guid { get; set; }

        public override MultipartFormDataContent MapMultipart()
        {
            return this.GetMultiparContent();
        }
    }
}
