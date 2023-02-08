using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class UserLoginHistoryCommandRepository : IUserLoginHistoryCommandRepository
    {

        private IDapperBase<UserLoginHistoryDto> DapperContext { get; set; }
        public UserLoginHistoryCommandRepository(IDapperBase<UserLoginHistoryDto> dapperContext)
        {
            DapperContext = dapperContext;
        }

        public Task<int> CreateUserLoginHistoryAsync(UserLoginHistoryDto userLoginHistoryModel)
        {
            return DapperContext.SaveAsync(SqlStatement.User_CreateUserLoginHistory, userLoginHistoryModel);
        }
    }
}
