using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class UserCommandRepository : IUserCommandRepository
    {
        public UserCommandRepository(IDapperBase<UserModel> dapperUser)
        {
            DapperUser = dapperUser;
        }

        private IDapperBase<UserModel> DapperUser { get; set; }

        public Task<int> CreateUserAsync(CreateUserDto user)
        {
            return DapperUser.SaveAsync(SqlStatement.User_CreateUser, user);
        }

        public async Task CreateUserRoleAsync(CreateUserRoleDto userRole)
        {
            await DapperUser.SaveAsync(SqlStatement.User_CreateUserRole, userRole);
        }

        public Task<int> EditUserAsync(EditUserDto editUserDto)
        {
            return DapperUser.EditAsync(SqlStatement.User_EditUser, editUserDto);
        }

        public Task EditUserRoleAsync(EditUserRoleDto userRole)
        {
            return DapperUser.SaveAsync(SqlStatement.User_EditUserRole, userRole);
        }

        public Task<int> UpdatePasswordAsync(UpdatePasswordDto updatePasswordDto)
        {
            return DapperUser.EditAsync(SqlStatement.User_UpdatePassword, updatePasswordDto);
        }

        public Task<int> UpdateUserStatusAsync(UpdateUserStatusDto updateUserStatusDto)
        {
            return DapperUser.EditAsync(SqlStatement.User_UpdateStatus, updateUserStatusDto);
        }

        public Task<int> SetTemporalPasswordAsync(int userId, string temporalPassword)
		{
            var parameters = new { userId, temporalPassword };
            return DapperUser.EditAsync(SqlStatement.User_SetTemporalPassword, parameters);
        }

        public Task<int> DeleteUserAsync(int userId)
        {
            var parameters = new { userId };
            return DapperUser.DeleteAsync(SqlStatement.User_DeleteUser, parameters);
        }
    }
}
