using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Enums;

namespace Authenticity.CourtSide.WebApp.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private const string USER_CREATION_WARNING_GUID_MESSAGE =
			"Currently, User GUID can not be retrieved from Autheniticy API.<br> Please set it manually on the <strong>Edit User</strong> option";
		public UserController(IUserDomain userService, ILogger<UserController> logger)
		{
			UserService = userService;
			Logger = logger;
		}
		private IUserDomain UserService { get; }
		private ILogger<UserController> Logger { get; }

		[HttpPost]
		[Route("CreateUser")]
		[Authorize]
		public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUserDto)
		{
			try
			{
				string responseMessage = string.Empty;

				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user data." });
				}

				var userRequest = TokenGenerator.GetUserByRequest(Request);
				createUserDto.CreatedBy = userRequest.Id;

				var createdUser = await UserService.CreateUserAsync(createUserDto);

				if (createUserDto.RoleId == (int)RoleEnum.Standard && string.IsNullOrWhiteSpace(createdUser.ApiGuid))
				{
					responseMessage = USER_CREATION_WARNING_GUID_MESSAGE;
				}

				BaseResponse<UserModel> response = new BaseResponse<UserModel>()
				{
					Data = createdUser,
					Success = true,
					Message = responseMessage
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				string customError = ex.Message;
				if (ex.Message.ToLower().Contains(ErrorResponseHelper.GENERIC_ERROR_INSERT_DUPLICATED_KEY))
				{
					customError = $"A user with the email '{createUserDto.Email}' already exists.";
				}
				var invalidOperationException = ErrorResponseHelper.GetCustomErrorResponse(ex, "CreateUserAsync", Logger, customError);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetUserById/{userId}")]
		[Authorize]
		public async Task<IActionResult> GetUserByIdAsync(int userId)
		{
			try
			{
				var user = await UserService.GetUserByIdAsync(userId);

				BaseResponse<UserModel> response = new BaseResponse<UserModel>()
				{
					Data = user,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetUserByIdAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetAllUsers")]
		[Authorize]
		public async Task<IActionResult> GetAllUsersAsync()
		{
			try
			{
				var users = await UserService.GetAllUsersAsync();

				BaseResponse<IEnumerable<UserModel>> response = new BaseResponse<IEnumerable<UserModel>>()
				{
					Data = users,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllUsersAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetAllRoles")]
		[Authorize]
		public async Task<IActionResult> GetAllRolesAsync()
		{
			try
			{
				var roles = await UserService.GetAllRolesAsync();

				BaseResponse<IEnumerable<RoleModel>> response = new BaseResponse<IEnumerable<RoleModel>>()
				{
					Data = roles,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllRolesAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPut]
		[Route("EditUser")]
		[Authorize]
		public async Task<IActionResult> EditUserAsync([FromBody] EditUserDto editUserDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user data." });
				}

				var userRequest = TokenGenerator.GetUserByRequest(Request);
				editUserDto.LastModifiedBy = userRequest.Id;

				var user = await UserService.EditUserAsync(editUserDto);

				BaseResponse<UserModel> response = new BaseResponse<UserModel>()
				{
					Data = user,
					Success = true
				};

				return Ok(response);
			}
			catch (ApplicationException ex)
			{
				var applicationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
				return BadRequest(applicationException);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "EditUserAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("SetPassword")]
		public async Task<IActionResult> SetPasswordAsync([FromBody] SetPasswordDto setPasswordDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user setup." });
				}

				bool userSetUp = await UserService.SetPasswordAsync(setPasswordDto);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Data = userSetUp,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "SetPasswordAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("ResetPassword/{email}")]
		public async Task<IActionResult> ResetPasswordAsync(string email)
		{
			try
			{
				await UserService.ResetPasswordAsync(email);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Success = true
				};

				return Ok(response);
			}
			catch (ApplicationException ex)
			{
				var applicationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
				return BadRequest(applicationException);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "ResetPasswordAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("CheckUserAPICredentials")]
		[Authorize]
		public async Task<IActionResult> CheckUserAPICredentialsAsync(UserApiCredentials userApiCredentials)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user API credentials." });
				}
				bool checkResponse = await UserService.CheckUserAPICredentialsAsync(userApiCredentials);


				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Data = checkResponse,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CheckUserAPICredentialsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("GetNewUserGuid")]
		[Authorize]
		public async Task<IActionResult> GetNewUserGuidAsync(string apiUrl)
		{
			try
			{
				string userGuid = await UserService.GetNewUserGuidAsync(apiUrl);

				BaseResponse<string> response = new BaseResponse<string>()
				{
					Data = userGuid,
					Success = true
				};

				return Ok(response);
			}
			catch (ApplicationException ex)
			{
				var applicationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
				return BadRequest(applicationException);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CheckUserAPICredentialsAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user API credentials." });
				}

				var userRequest = TokenGenerator.GetUserByRequest(Request);

				changePasswordDto.UserId = userRequest.Id;

				await UserService.ChangePasswordAsync(changePasswordDto);

				BaseResponse<string> response = new BaseResponse<string>()
				{
					Success = true
				};

				return Ok(response);
			}
			catch (ApplicationException ex)
			{
				var applicationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
				return BadRequest(applicationException);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "ChangePasswordAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpPost]
		[Route("PasswordRecovery")]
		public async Task<IActionResult> PasswordRecoveryAsync(PasswordRecoveryDto passwordRecoveryDto)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
					return BadRequest(new { success = false, errors, message = "Invalid user password recovery data." });
				}

				await UserService.PasswordRecoveryAsync(passwordRecoveryDto);

				BaseResponse<bool> response = new BaseResponse<bool>()
				{
					Success = true
				};

				return Ok(response);
			}
			catch (ApplicationException ex)
			{
				var applicationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
				return BadRequest(applicationException);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "ForgotPasswordAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}


		[HttpDelete]
		[Route("DeleteUser/{userId}")]
		[Authorize]
		public async Task<IActionResult> DeleteUserAsync(int userId)
		{
			try
			{
				var userDeleted = await UserService.DeleteUserAsync(userId);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = userDeleted,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				string customError = ex.Message;
				var invalidOperationException = ErrorResponseHelper.GetCustomErrorResponse(ex, "DeleteUserAsync", Logger, customError);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpDelete]
		[Route("DeleteStandardUser/{userId}")]
		[Authorize]
		public async Task<IActionResult> DeleteStandardUser(int userId)
		{
			try
			{
				var userDeleted = await UserService.DeleteUserAsync(userId);

				BaseResponse<int> response = new BaseResponse<int>()
				{
					Data = userDeleted,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				string customError = ex.Message;
				var invalidOperationException = ErrorResponseHelper.GetCustomErrorResponse(ex, "DeleteStandardUser", Logger, customError);
				return BadRequest(invalidOperationException);
			}
		}

		[HttpGet]
		[Route("GetAllTranscriptionEngines")]
		[Authorize]
		public async Task<IActionResult> GetAllTranscriptionEnginesAsync()
		{
			try
			{
				IEnumerable<TranscriptionEngine> transcriptionEngineList = await UserService.GetAllTranscriptionEnginesAsync();

				BaseResponse<IEnumerable<TranscriptionEngine>> response = new BaseResponse<IEnumerable<TranscriptionEngine>>()
				{
					Data = transcriptionEngineList,
					Success = true
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				var invalidOperationException = ErrorResponseHelper.GetCustomErrorResponse(ex, "GetAllTranscriptionEnginesAsync", Logger);
				return BadRequest(invalidOperationException);
			}
		}
	}
}
