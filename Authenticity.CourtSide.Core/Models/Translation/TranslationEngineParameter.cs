using System.Net.Http;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
    public abstract class TranslationEngineParameter
    {
        public abstract MultipartFormDataContent MapMultipart();
    }
}
