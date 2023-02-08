using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Authenticity.CourtSide.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxRequestBodySize = 524288000;
                        serverOptions.Limits.MaxRequestBufferSize = null;
                    })
                    .UseStartup<Startup>();
                });
    }
}
