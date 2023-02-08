using Authenticity.CourtSide.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class AuthenticationPathConfiguration
    {
        private static string Secret = "CourtSide Secret Key";

        private static string Aud = "http://localhost:61160";

        private static string Iss = "http://localhost:61160";

        public static AppAudience GetAudience(int expirationTimeInMinutes)
        {
            return new AppAudience()
            {
                Secret = Secret,
                Aud = Aud,
                Iss = Iss,
                ExpireTimeInMinutes = expirationTimeInMinutes
            };
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Secret));

            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Iss,
                ValidateAudience = true,
                ValidAudience = Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
        }
    }
}
