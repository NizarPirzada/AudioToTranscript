using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class UserLoginHistoryQueryRepository : IUserLoginHistoryQueryRepository
    {
        private IDapperBase<UserLoginHistoryDto> DapperContext { get; set; }
        public UserLoginHistoryQueryRepository(IDapperBase<UserLoginHistoryDto> dapperContext)
        {
            DapperContext = dapperContext;
        }
        public Task<UserLoginHistoryDto> GetLastUserLoginStoryByUserIdAsync(int userId)
        {
            var parameters = new { userId };
            return DapperContext.GetAsync<UserLoginHistoryDto>(SqlStatement.User_GetUserLoginHistoryByUserId, parameters);
        }
    }
}
