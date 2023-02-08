using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface IUserQueryRepository
    {
        Task<UserModel> GetUserByIdAsync(int userId);
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByEmailActivationIdAsync(string emailActivationId);
        Task<bool> CheckTemporalPasswordAsync(int userId, string temporalPassword);
    }
}
