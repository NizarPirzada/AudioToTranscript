using Authenticity.CourtSide.Core.Models.Transcription;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders
{
	public interface ITranslationEngine
    {
        Task<string> CreateTranslationRequestAsync(TranslationRequestModel translationRequest);
        Task<Stream> CheckTranslationResponseAsync(string serverUrl, string taskId);
    }
}
