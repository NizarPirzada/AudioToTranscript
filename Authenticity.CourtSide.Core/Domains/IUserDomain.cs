using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface IUserDomain
	{
		Task<UserModel> CreateUserAsync(CreateUserDto createUserDto);
		Task<UserModel> GetUserByIdAsync(int userId);
		Task<IEnumerable<UserModel>> GetAllUsersAsync();
		Task<UserModel> EditUserAsync(EditUserDto user);
		Task<IEnumerable<RoleModel>> GetAllRolesAsync();
		Task<bool> SetPasswordAsync(SetPasswordDto setPasswordDto);
		Task ResetPasswordAsync(string email);
		Task<bool> CheckUserAPICredentialsAsync(UserApiCredentials userApiCredentials);
		Task<string> GetNewUserGuidAsync(string apiUrl);
		Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);
		Task PasswordRecoveryAsync(PasswordRecoveryDto passwordRecoveryDto);
		Task<int> DeleteUserAsync(int userId);
		Task<IEnumerable<TranscriptionEngine>> GetAllTranscriptionEnginesAsync();
	}
}
