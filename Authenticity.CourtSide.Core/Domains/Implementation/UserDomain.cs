using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Resources;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Microsoft.Extensions.Logging;
using Authenticity.CourtSide.Core.DataProviders.Implementation;
using Microsoft.AspNetCore.SignalR;
using Authenticity.CourtSide.Core.Utilities.SignalR;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class UserDomain : IUserDomain
	{
		private const string INCORRECT_EMAIL_OR_TEMP_PASSWORD = "Incorrect Email or Temporary Password";
		private const string INCORRECT_EMAIL = "Incorrect Email";
		private ILogger<UserDomain> _logger;
		private IHubContext<AdminNotificationHub> _notificationHub { get; }

		public IUserContext UserContext { get; }

		public UserDomain(IUserContext userContext, ILogger<UserDomain> logger, IHubContext<AdminNotificationHub> notificationHub)
		{
			UserContext = userContext;
			_logger = logger;
			_notificationHub = notificationHub;
		}

		public async Task<UserModel> CreateUserAsync(CreateUserDto user)
		{
			user.Email = user.Email.Replace(" ", "");

			Guid emailActivationId = Guid.NewGuid();
			user.EmailActivationId = emailActivationId.ToString();

			if (user.RoleId == (int)RoleEnum.Standard)
			{
				string transcriptAPIUrl = UserContext.Configuration["TranscriptionApiURL"];

				if (string.IsNullOrWhiteSpace(transcriptAPIUrl))
				{
					throw new ArgumentNullException("TranscriptionApiURL application settings value no found");
				}

				string userGuid = await UserContext.AuthenticationDataProvider.GetUserGuid(transcriptAPIUrl);
				user.ApiUrl = transcriptAPIUrl;
				user.ApiGuid = userGuid;
			}
			else
			{
				user.TranscriptionEngineId = null;
			}

			int createdUserId = await UserContext.UserCommandRepository.CreateUserAsync(user);

			var userRoleDto = new CreateUserRoleDto()
			{
				UserId = createdUserId,
				RoleId = user.RoleId,
				CreatedBy = user.CreatedBy
			};

			await UserContext.UserCommandRepository.CreateUserRoleAsync(userRoleDto);

			UserModel createdUser = await UserContext.UserQueryRepository.GetUserByIdAsync(createdUserId);
			UserContext.EmailDomain.SendUserRegisterEmail(createdUser);

			return createdUser;
		}

		public async Task<UserModel> GetUserByIdAsync(int userId)
		{
			var response = await UserContext.UserQueryRepository.GetUserByIdAsync(userId);
			return response;
		}

		public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
		{
			var users = await UserContext.UserQueryRepository.GetAllUsersAsync();

			foreach (UserModel user in users)
			{
				await GetLastUserLoginAsync(user);
			}

			return users;
		}

		public async Task<IEnumerable<RoleModel>> GetAllRolesAsync()
		{
			var response = await UserContext.RoleQueryRepository.GetAllRolesAsync();
			return response;
		}

		public async Task<UserModel> EditUserAsync(EditUserDto user)
		{
			UserModel currentUser = await UserContext.UserQueryRepository.GetUserByIdAsync(user.Id);

			if (currentUser.Roles.FirstOrDefault()?.RoleId == (int)RoleEnum.Standard)
			{
				TranscriptionEngine transcriptionEngine = await UserContext.TranscriptionEngineQueryRepository.GetTranscriptionEngineByIdAsync(user.TranscriptionEngineId);
				UserApiCredentials userApiCredentials = new UserApiCredentials()
				{
					ApiUrl = user.ApiUrl,
					ApiGuid = user.ApiGuid,
					ApiEngine = transcriptionEngine.Code
				};

				await UserContext.AuthenticationDataProvider.CheckUserAPICredentialsAsync(userApiCredentials);
			}

			if (!currentUser.Email.Equals(user.Email))
			{
				user.EmailActivationId = Guid.NewGuid().ToString();
			}

			await UserContext.UserCommandRepository.EditUserAsync(user);

			UserModel editedUser = await UserContext.UserQueryRepository.GetUserByIdAsync(user.Id);

			await GetLastUserLoginAsync(editedUser);

			return editedUser;
		}

		public async Task<bool> SetPasswordAsync(SetPasswordDto setPasswordDto)
		{
			UserModel user = await UserContext.UserQueryRepository.GetUserByEmailActivationIdAsync(setPasswordDto.EmailActivationId);

			string hashedPassword = UserContext.PasswordHasher.Hash(setPasswordDto.Password);

			UpdatePasswordDto updatePasswordDto = new UpdatePasswordDto
			{
				Id = user.Id,
				Password = hashedPassword,
				LastModifiedBy = user.Id
			};

			await UserContext.UserCommandRepository.UpdatePasswordAsync(updatePasswordDto);

			UpdateUserStatusDto updateUserStatusDto = new UpdateUserStatusDto
			{
				Id = user.Id,
				Status = UserStatusEnum.Active,
				LastModifiedBy = user.Id
			};

			await UserContext.UserCommandRepository.UpdateUserStatusAsync(updateUserStatusDto);

			return true;
		}

		public async Task ResetPasswordAsync(string email)
		{
			UserModel user = await UserContext.UserQueryRepository.GetUserByEmailAsync(email);

			if (user == null)
			{
				throw new ApplicationException(INCORRECT_EMAIL);
			}

			string temporalPassword = Guid.NewGuid().ToString().Substring(0, 8);

			await UserContext.UserCommandRepository.SetTemporalPasswordAsync(user.Id, temporalPassword);

			UserContext.EmailDomain.SendPasswordRecoveryEmail(user, temporalPassword);
		}

		public Task<bool> CheckUserAPICredentialsAsync(UserApiCredentials userApiCredentials)
		{
			return UserContext.AuthenticationDataProvider.CheckUserAPICredentialsAsync(userApiCredentials);
		}

		public Task<string> GetNewUserGuidAsync(string apiUrl)
		{
			return UserContext.AuthenticationDataProvider.GetUserGuid(apiUrl);
		}

		public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
		{
			UserModel user = await UserContext.UserQueryRepository.GetUserByIdAsync(changePasswordDto.UserId);

			bool currentPassIsValid = UserContext.PasswordHasher.Check(user.Password, changePasswordDto.CurrentPassword);

			if (!currentPassIsValid)
			{
				throw new ApplicationException("Invalid current password");
			}

			string hashedPassword = UserContext.PasswordHasher.Hash(changePasswordDto.NewPassword);

			UpdatePasswordDto updatePasswordDto = new UpdatePasswordDto
			{
				Id = user.Id,
				Password = hashedPassword,
				LastModifiedBy = user.Id
			};

			await UserContext.UserCommandRepository.UpdatePasswordAsync(updatePasswordDto);
		}

		public async Task PasswordRecoveryAsync(PasswordRecoveryDto passwordRecoveryDto)
		{
			UserModel user = await UserContext.UserQueryRepository.GetUserByEmailAsync(passwordRecoveryDto.Email);

			if (user == null)
			{
				throw new ApplicationException(INCORRECT_EMAIL_OR_TEMP_PASSWORD);
			}

			bool temporalPasswordIsValid = await UserContext.UserQueryRepository.CheckTemporalPasswordAsync(user.Id, passwordRecoveryDto.TemporalPassword);

			if (!temporalPasswordIsValid)
			{
				throw new ApplicationException(INCORRECT_EMAIL_OR_TEMP_PASSWORD);
			}

			string hashedPassword = UserContext.PasswordHasher.Hash(passwordRecoveryDto.Password);

			UpdatePasswordDto updatePasswordDto = new UpdatePasswordDto
			{
				Id = user.Id,
				Password = hashedPassword,
				LastModifiedBy = user.Id
			};

			await UserContext.UserCommandRepository.UpdatePasswordAsync(updatePasswordDto);

			await UserContext.UserCommandRepository.SetTemporalPasswordAsync(user.Id, string.Empty);
		}

		private async Task GetLastUserLoginAsync(UserModel user)
		{
			UserLoginHistoryDto userLoginStory = await UserContext.UserLoginHistoryQueryRepository.GetLastUserLoginStoryByUserIdAsync(user.Id);

			if (userLoginStory != null)
			{
				user.LastLogin = userLoginStory.LoginTime;
			}
		}

		public async Task<int> DeleteUserAsync(int userId)
		{
            UserModel user = await UserContext.UserQueryRepository.GetUserByIdAsync(userId);
			// If user is a Standard user
			if (user.Roles.FirstOrDefault().RoleId == 2) 
			{
                IEnumerable<TranscriptFile> userFiles =
                (await UserContext.TranscriptFileQueryRepository.GetAllTranscriptFileByUserIdAsync(userId));

                IFileProvider fileProvider = await UserContext.FileProviderFactory.CreateAsync();
				
				await _notificationHub.Clients.All.SendAsync("NotifyDeletionStep", $"Delete media {DateTime.Now.ToLongDateString()}", "Deleting media files...");

				foreach (TranscriptFile file in userFiles)
                {
                    try
                    {
                        await fileProvider.DeleteFileAsync(file.Path);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"ERROR Deleting media files of user {userId} - {file.Name}");
                    }
                }
				
				await _notificationHub.Clients.All.SendAsync("NotifyDeletionStep", $"Media files deleted {DateTime.Now.ToLongDateString()}", "Media files deleted");

				var userTranscripts = await UserContext.TranscriptQueryRepository.GetAllTranscriptsByUserAsync(userId);
                foreach (Transcript transcript in userTranscripts)
                {
                    try
                    {
                        await UserContext.TranscriptDialogCommandRepository.DeleteDialogsByTranscriptIdAsync(transcript.Id);
                        await UserContext.TranscriptPersonCommandRepository.DeleteAllPeopleByTranscriptIdAsync(transcript.Id);
                        await UserContext.CourtCommandRepository.DeleteCourtByTranscriptIdAsync(transcript.Id);
                        await UserContext.TranscriptJobCommandRepository.DeleteAllJobsByTranscriptIdAsync(transcript.Id);
                        await UserContext.TranscriptRecordingInfoCommandRepository.DeleteTranscriptRecordingInfoByTranscriptIdAsync(transcript.Id);
                        await UserContext.TranscriptCommandRepository.DeleteAsync(transcript.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"ERROR Deleting transcript information of user {userId} - {transcript.Name}");
                    }
                }
				
				await _notificationHub.Clients.All.SendAsync("NotifyDeletionStep", $"Transcriptions deleted {DateTime.Now.ToLongDateString()}", "Transcriptions deleted");
			}
			var response = await UserContext.UserCommandRepository.DeleteUserAsync(userId);
			return response;
		}

		public Task<IEnumerable<TranscriptionEngine>> GetAllTranscriptionEnginesAsync()
		{
			return UserContext.TranscriptionEngineQueryRepository.GetAllTranscriptionEnginesAsync();
		}
	}
}
