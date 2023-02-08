using Authenticity.CourtSide.Core.Models.Transcription;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders
{
	public interface ITranscriptionEngine
    {
        Task<string> CreateTranscriptRequestAsync(TranscriptRequestModel transcriptRequest);
        Task<string> CheckTranscriptResponseAsync(string serverUrl, string taskId);
    }
}
