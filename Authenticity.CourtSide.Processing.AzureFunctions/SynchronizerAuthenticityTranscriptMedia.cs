using Authenticity.CourtSide.Core.Domains;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Processing.AzureFunctions
{
    public static class SynchronizerAuthenticityTranscriptMedia
    {
        [FunctionName("synchronizer-authenticity-transcript-media")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            string message = string.Empty;
            log.LogInformation($"Starting the sync process at: {DateTime.Now}");

            try
            {
                var startup = new Startup(log, context);
                var transcriptionProvidersDomain = startup.Provider.GetRequiredService<ITranscriptionProvidersDomain>();

                log.LogInformation($"Sending the media files to Authenticity API at: {DateTime.Now}");
                var totalJobsSent = await transcriptionProvidersDomain.SendNewJobsToTranscriptionEngineAsync();
                message = $"Total media files sent to Authenticity API -- {totalJobsSent}";
                log.LogInformation(message);


                log.LogInformation($"Retrieving transcripts from Authenticity API at: {DateTime.Now}");
                var transcriptionsCompleted = await transcriptionProvidersDomain.CheckPendingJobsFromTranscriptionEngineAsync();
                message = $"Total transcriptions completed from Authenticity API -- {transcriptionsCompleted}";
            }
            catch (Exception ex)
            {
                log.LogError("An error ocurrend during the execution of the process - {0}", ex.Message);
            }
        }
    }
}
