using System.Net.Http;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
    public abstract class TranscriptEngineParameter
    {
        public abstract MultipartFormDataContent MapMultipart();
    }
}
