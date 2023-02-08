using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Resources;
using Microsoft.Extensions.Configuration;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
    public class EmailDomain: IEmailDomain
    {
        private EmailConfiguration EmailConfiguration { get; }
        private IConfiguration Configuration { get; }
        public EmailDomain(EmailConfiguration emailConfiguration, IConfiguration configuration)
        {
            EmailConfiguration = emailConfiguration;
            Configuration = configuration;
        }

        public void SendUserRegisterEmail(UserModel userModel)
        {
            string template = EmailTemplates.UserSetupBody;
            string clientHost = Configuration["ClientHost"].EndsWith("/") ? $"{Configuration["ClientHost"]}#/" : $"{Configuration["ClientHost"]}/#/";
            template = template.Replace("[FirstName]", userModel.FirstName)
                .Replace("[LastName]", userModel.LastName)
                .Replace("[Url]", $"{clientHost}ResetPassword/{userModel.EmailActivationId}");

            EmailHelper.SendEmailOffice365(EmailConfiguration, userModel.Email, template, "User setup");
        }

        public void SendPasswordRecoveryEmail(UserModel userModel, string temporaryPassword)
        {
            var template = EmailTemplates.UserForgotPasswordBody;
            string clientHost = Configuration["ClientHost"].EndsWith("/") ? $"{Configuration["ClientHost"]}#/" : $"{Configuration["ClientHost"]}/#/";
            template = template.Replace("[FirstName]", userModel.FirstName)
                .Replace("[LastName]", userModel.LastName)
                .Replace("[TemporaryPassword]", temporaryPassword)
                .Replace("[Url]", $"{clientHost}Login/passwordRecovery");

            EmailHelper.SendEmailOffice365(EmailConfiguration, userModel.Email, template, "Password Recovery");
        }
    }
}
