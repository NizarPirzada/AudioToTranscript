using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.Context;
using Authenticity.CourtSide.Core.DataProviders.Context.Implementation;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.DataProviders.FileProvider.Implementation;
using Authenticity.CourtSide.Core.DataProviders.Implementation;
using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Domains.Context.Implementation;
using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.ORM.Implementation;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Authenticity.CourtSide.Core.Repositories.Query;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Authenticity.CourtSide.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using Authenticity.CourtSide.Core.DataProviders.Context;
using Authenticity.CourtSide.Core.DataProviders.Context.Implementation;

namespace Authenticity.CourtSide.Processing.ConsoleApp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider provider;

        public IServiceProvider Provider => provider;
        public IConfiguration Configuration => configuration;

        public Startup()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environment}.json", optional: true)
                            .AddEnvironmentVariables()
                            .Build();
            
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddTransient(typeof(IDapperBase<>), typeof(DapperBase<>));

            services.AddSingleton<IHttpClientFactory, CourtSideHttpClient>();
            services.AddScoped<ILogger, Logger<TranscriptionProvidersDomainContext>>();
            services.AddScoped<ITranscriptJobQueryRepository, TranscriptJobQueryRepository>();
            services.AddScoped<ITranscriptFileQueryRepository, TranscriptFileQueryRepository>();
            services.AddScoped<ITranscriptJobCommandRepository, TranscriptJobCommandRepository>();
            services.AddScoped<ITranscriptDialogCommandRepository, TranscriptDialogCommandRepository>();
            services.AddScoped<ISettingsQueryRepository, SettingsQueryRepository>();
            services.AddScoped<ITranscriptCommandRepository, TranscriptCommandRepository>();
            services.AddScoped<IFileProviderFactory, FileProviderFactory>();
            services.AddScoped<IFileProviderFactoryContext, FileProviderFactoryContext>();
            services.AddSingleton<ITranscriptionEngine, AuthenticityTranscriptionEngine>();
            services.AddScoped<ITranscriptionProvidersDomainContext, TranscriptionProvidersDomainContext>();
            services.AddSingleton<ITranscriptionProvidersDomain, TranscriptionProvidersDomain>();
            services.AddSingleton<ISettingsCommandRepository, SettingsCommandRepository>();
            services.AddSingleton<IUserQueryRepository, UserQueryRepository>();
            services.AddScoped<ITranscriptQueryRepository, TranscriptQueryRepository>();
            services.AddScoped<ILanguageQueryRepository, LanguageQueryRepository>();

            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<TranscriptionProvidersDomainContext>();

            provider = services.BuildServiceProvider();

        }
    }
}
