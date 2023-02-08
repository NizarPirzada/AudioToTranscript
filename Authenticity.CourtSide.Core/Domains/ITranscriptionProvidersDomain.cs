using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface ITranscriptionProvidersDomain : IDomain
    {
		Task<int> SendNewJobsToTranscriptionEngineAsync();
        Task<int> CheckPendingJobsFromTranscriptionEngineAsync();
    }
}
