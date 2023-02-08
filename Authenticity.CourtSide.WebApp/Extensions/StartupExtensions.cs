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
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.ORM.Implementation;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Authenticity.CourtSide.Core.Repositories.Query;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Authenticity.CourtSide.Core.Utilities;
using Authenticity.CourtSide.Core.Utilities.SignalR;
using Authenticity.CourtSide.Core.Utilities.SignarlR;
using Authenticity.CourtSide.Core.Utilities.SignarlR.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;

namespace Authenticity.CourtSide.WebApp.Extensions
{
	public static class StartupExtensions
    {
        public static void ConfigureServicesForContexts(this IServiceCollection services) 
        {
            services.AddScoped<ITranscriptContext, TranscriptContext>();
            services.AddScoped<IExportContext, ExportContext>();
            services.AddScoped<IFileContext, FileContext>();
            services.AddScoped<IFileProviderFactoryContext, FileProviderFactoryContext>();
            services.AddScoped<IUserContext, UserContext>();
        }
        public static void ConfigureServicesForDomains(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationDomain, AuthenticationDomain>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<IPermissionDomain, PermissionDomain>();
            services.AddScoped<ITranscriptDomain, TranscriptDomain>();
            services.AddScoped<IFileDomain, FileDomain>();
            services.AddScoped<IEmailDomain, EmailDomain>();
            services.AddScoped<IExportDomain, ExportDomain>();
            services.AddScoped<ISettingsDomain, SettingsDomain>();
        }

        public static void ConfigureServicesForRepositories(this IServiceCollection services)
        {
            services.AddScoped<IHttpClientFactory, CourtSideHttpClient>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();
            services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();
            services.AddScoped<ITranscriptQueryRepository, TranscriptQueryRepository>();
            services.AddScoped<ITranscriptCommandRepository, TranscriptCommandRepository>();
            services.AddScoped<ITranscriptPersonQueryRepository, TranscriptPersonQueryRepository>();
            services.AddScoped<ITranscriptPersonCommandRepository, TranscriptPersonCommandRepository>();
            services.AddScoped<ITranscriptFileQueryRepository, TranscriptFileQueryRepository>();
            services.AddScoped<ITranscriptFileCommandRepository, TranscriptFileCommandRepository>();
            services.AddScoped<ITranscriptJobCommandRepository, TranscriptJobCommandRepository>();
            services.AddScoped<ITranscriptDialogQueryRepository, TranscriptDialogQueryRepository>();
            services.AddScoped<ITranscriptJobQueryRepository, TranscriptJobQueryRepository>();
            services.AddScoped<ITranscriptDialogCommandRepository, TranscriptDialogCommandRepository>();
            services.AddScoped<IFileProviderFactory, FileProviderFactory>();
            services.AddScoped<LocalFileHelper>();
            services.AddScoped<IUserLoginHistoryQueryRepository, UserLoginHistoryQueryRepository>();
            services.AddScoped<IUserLoginHistoryCommandRepository, UserLoginHistoryCommandRepository>();
            services.AddScoped<IFileExportFormatQueryRepository, FileExportFormatQueryRepository>();
            services.AddScoped<IFileExporter, WordDocumentExporter>();
            services.AddScoped<ICourtQueryRepository, CourtQueryRepository>();
            services.AddScoped<ICourtCommandRepository, CourtCommandRepository>();
            services.AddScoped<IPersonAdditionalInfoCommandRepository, PersonAdditionalInfoCommandRepository>();
            services.AddScoped<ISettingsQueryRepository, SettingsQueryRepository>();
            services.AddScoped<ISettingsCommandRepository, SettingsCommandRepository>();
            services.AddScoped<IAuthenticationDataProvider, AuthenticityAuthenticationDataProvider>();
            services.AddScoped<ITranscriptRecordingInfoQueryRepository, TranscriptRecordingInfoQueryRepository>();
            services.AddScoped<ITranscriptRecordingInfoCommandRepository, TranscriptRecordingInfoCommandRepository>();
            services.AddScoped<ITranslationEngine, AuthenticityTranslationEngine>();
            services.AddTransient<IAdminNotificationHub, AdminNotificationHub>();
            services.AddTransient<ITranscriptionNotificationHub, TranscriptionNotificationHub>();
            services.AddScoped<ILanguageQueryRepository, LanguageQueryRepository>();
            services.AddScoped<ITranscriptionEngineQueryRepository, TranscriptionEngineQueryRepository>();
        }

        public static void ConfigureServicesForDapper(this IServiceCollection services)
        {
            bool isIntegration = services.Any(c => typeof(IDapperBase<>) == c.ServiceType);
            if (!isIntegration)
            {
                services.AddTransient(typeof(IDapperBase<>), typeof(DapperBase<>));
            }
        }

        public static void AddConfiguration<T>(this IServiceCollection services, IConfiguration configuration, string configurationTag = null) where T : class
        {
            if (string.IsNullOrEmpty(configurationTag))
            {
                configurationTag = typeof(T).Name;
            }

            var instance = Activator.CreateInstance<T>();
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(configurationTag)).Configure(instance);
            services.AddSingleton(instance);
        }
    }
}
