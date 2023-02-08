using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class TokenGenerator
    {
        /// <summary>
        /// JWT Token generator class using "secret-key"
        /// more info: https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html
        /// </summary>
        public static LoginResponseDto GenerateTokenJwt(UserModel user, int expirationTimeInMinutes)
        {
            AppAudience _settings = AuthenticationPathConfiguration.GetAudience(expirationTimeInMinutes);

            var secretKey = _settings.Secret;
            var audienceToken = _settings.Aud;
            var issuerToken = _settings.Iss;
            var expireTime = _settings.ExpireTimeInMinutes;

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ICollection<Claim> claims = new List<Claim>() {
                new Claim(CourtsideClaimTypes.Id, user.Id.ToString()),
                new Claim(CourtsideClaimTypes.FirstName, user.FirstName.ToString()),
                new Claim(CourtsideClaimTypes.LastName, user.LastName.ToString()),
            };

            if (user.Roles.Any())
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(CourtsideClaimTypes.Role, role.Name));
                }
            }

            // create token to the user
            var tokenHandler = new JwtSecurityTokenHandler();

            var expirationToken = DateTime.Now.AddMinutes(expireTime);

            var jwtSecurityToken = new JwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expirationToken,
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            LoginResponseDto responseToken = new LoginResponseDto()
            {
                Token = jwtTokenString,
                Expires = expirationToken
            };

            return responseToken;
        }

        public static IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Given token is null or empty");
            }

            TokenValidationParameters tokenValidationParameters = AuthenticationPathConfiguration.GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetClaimValue(string bearerToken, string claimType)
        {
            var claims = GetTokenClaims(bearerToken);
            return claims.FirstOrDefault(x => x.Type == claimType).Value;
        }

        public static UserModel GetUserByRequest(HttpRequest request)
        {
            var tokenHeader = request.Headers["Authorization"];
            var token = tokenHeader.First().Split(' ');
            var claims = GetTokenClaims(token[1]);

            var roles = claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(r => new RoleModel { Name = r.Value }).ToList();

            return new UserModel
            {
                FirstName = claims.FirstOrDefault(x => x.Type == CourtsideClaimTypes.FirstName).Value,
                LastName = claims.FirstOrDefault(x => x.Type == CourtsideClaimTypes.LastName).Value,
                Id = int.Parse(claims.FirstOrDefault(x => x.Type == CourtsideClaimTypes.Id).Value),
                Roles = roles
            };
        }
    }
}
