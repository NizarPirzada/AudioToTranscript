using Authenticity.CourtSide.Core.Entities;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface IUserLoginHistoryQueryRepository
    {
        Task<UserLoginHistoryDto> GetLastUserLoginStoryByUserIdAsync(int userId);
    }
}
