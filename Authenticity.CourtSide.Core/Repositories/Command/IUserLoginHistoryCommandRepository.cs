using Authenticity.CourtSide.Core.Entities;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface IUserLoginHistoryCommandRepository
    {
        Task<int> CreateUserLoginHistoryAsync(UserLoginHistoryDto userLoginHistoryModel);
    }
}
