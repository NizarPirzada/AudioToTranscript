using Authenticity.CourtSide.Core.Helpers;
using System.IO;
using System.Net.Http;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
	public class AuthenticityTranslationEngineRequest : TranslationEngineParameter
    {
        public Stream file { get; set; }
        public string from_lang { get; set; }
        public string to_lang { get; set; }
        public string guid { get; set; }
        public string mode { get; set; }

        public override MultipartFormDataContent MapMultipart()
        {
            return this.GetMultiparContent();
        }
    }
}
