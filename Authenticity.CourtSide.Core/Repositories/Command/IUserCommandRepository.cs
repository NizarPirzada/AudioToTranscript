using Authenticity.CourtSide.Core.Entities;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface IUserCommandRepository
    {
        Task<int> CreateUserAsync(CreateUserDto createUserDto);
        Task CreateUserRoleAsync(CreateUserRoleDto userRole);
        Task<int> EditUserAsync(EditUserDto editUserDto);
        Task EditUserRoleAsync(EditUserRoleDto userRole);
        Task<int> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto);
        Task<int> UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto);
        Task<int> SetTemporalPasswordAsync(int userId, string temporaryPassword);
        Task<int> DeleteUserAsync(int userId);
    }
}
