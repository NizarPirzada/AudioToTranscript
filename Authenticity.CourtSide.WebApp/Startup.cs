using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Authenticity.CourtSide.Core.Utilities.SignalR;
using Authenticity.CourtSide.Core.Utilities.SignarlR.Implementation;

namespace Authenticity.CourtSide.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // MemoryCacheConfiguration
            services.AddConfiguration<MemoryCacheConfiguration>(Configuration, "MemoryCacheConfiguration");

            // EmailConfiguration
            services.AddConfiguration<EmailConfiguration>(Configuration, "EmailConfiguration");

            // Dapper 
            services.ConfigureServicesForDapper();

            // Repositories
            services.ConfigureServicesForRepositories();

            // Contexts
            services.ConfigureServicesForContexts();

            // Domains
            services.ConfigureServicesForDomains();

            services.AddMemoryCache();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = AuthenticationPathConfiguration.GetTokenValidationParameters();
           });

            services.AddCors();

            services.AddControllers();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AdminNotificationHub>("/admin-notification-hub");
                endpoints.MapHub<TranscriptionNotificationHub>("/transcription-notification-hub");
            });

            app.MapWhen(x => !x.Request.RouteValues.ContainsKey("controller"), builder =>
            {
                builder.UseSpa(spa =>
                {
                    spa.Options.DefaultPage.Add("/login");
                });
            });
        }
    }
}
