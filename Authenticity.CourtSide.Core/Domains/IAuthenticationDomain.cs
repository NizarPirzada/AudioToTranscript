using Authenticity.CourtSide.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface IAuthenticationDomain
    {
        Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto, int expirationTokenTime);
    }
}
