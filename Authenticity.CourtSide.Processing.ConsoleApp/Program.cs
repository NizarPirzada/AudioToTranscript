using Authenticity.CourtSide.Core.Domains;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Processing.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var startup = new Startup();

            Console.WriteLine(" ****************** COURTSIDE - AUTHENTICITY DATA SYNC *********************");
            Console.WriteLine("What do you would like to do?\n" +
                              "1-> Send all new jobs to Authenticity Transcription\n" +
                              "2-> Check status of jobs sent to Authentiticy (Get transcriptions)\n");

            var option = int.Parse(Console.ReadLine());


            try
            {
                var transcriptionProvidersDomain = startup.Provider.GetRequiredService<ITranscriptionProvidersDomain>();
                int totalJobsSent = 0;

                Console.WriteLine("Starting proccess...");

                switch (option)
                {
                    case 1:
                        totalJobsSent = await transcriptionProvidersDomain.SendNewJobsToTranscriptionEngineAsync().ConfigureAwait(false);
                        Console.WriteLine($"Total media files sent to Atuhenticity API: {totalJobsSent}");
                        break;
                    case 2:
                        totalJobsSent = await transcriptionProvidersDomain.CheckPendingJobsFromTranscriptionEngineAsync().ConfigureAwait(false);
                        Console.WriteLine($"Total transcriptions obtained from API: {totalJobsSent}");
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
