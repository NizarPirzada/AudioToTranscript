using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using Microsoft.Extensions.Logging;
using Authenticity.CourtSide.Core.Utilities.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
	[Trait("UnitTest", "UserDomain")]
	public class UserDomainTest
	{
		#region [Setup]
		private const int AFFECTED_ROWS = 1;
		private const string STANDARD_USER_GUID = "Fake Guid";
		private const string USER_CURRENT_PASSWORD_SUCCESS = "Fake Current User Password";
		private const string STANDARD_USER_URL = "Fake URL";
		private const string USER_SUCCESS_EMAIL = "Fake Success Email";
		private const string USER_TEMPORARY_PASSWORD = "Fake Temp password";
		private UserDomain InstanceToTest { get; set; }
		Mock<IUserCommandRepository> MockIUserCommandRepository { get; }
		private Mock<IAuthenticationDataProvider> MockIAuthenticationDataProvider { get; }

		public UserDomainTest()
		{
			List<UserModel> usersList = new List<UserModel>
			{
				new UserModel
				{
					Id = 1,
					Email = "fakeemail@fake.com",
					EmailActivationId = Guid.NewGuid().ToString(),
				}
			};

			List<RoleModel> rolesList = new List<RoleModel>
			{
				new RoleModel
				{
					RoleId = 1
				}
			};

			UserLoginHistoryDto userLoginHistoryModel = new UserLoginHistoryDto
			{
				Id = 1,
				UserId = 1,
				LoginTime = DateTime.Now
			};

			List<TranscriptionEngine> transcriptionEngineList = new List<TranscriptionEngine>
			{
				new TranscriptionEngine
				{
					Id = 1,
					Name = "Fake engine",
					Code = "Fake engine code"
				}
			};

			MockIUserCommandRepository = new Mock<IUserCommandRepository>();
			MockIAuthenticationDataProvider = new Mock<IAuthenticationDataProvider>();
			Mock<IUserQueryRepository> mockIUserQueryRepository = new Mock<IUserQueryRepository>();
			Mock<IRoleQueryRepository> mockIRoleQueryRepository = new Mock<IRoleQueryRepository>();
			Mock<IEmailDomain> mockIEmailDomain = new Mock<IEmailDomain>();
			Mock<IPasswordHasher> mockIPasswordHasher = new Mock<IPasswordHasher>();
			Mock<IUserLoginHistoryQueryRepository> mockIUserLoginHistoryQueryRepository = new Mock<IUserLoginHistoryQueryRepository>();
			Mock<IConfiguration> mockIConfiguration = new Mock<IConfiguration>();
			Mock<ITranscriptionEngineQueryRepository> mockITranscriptionEngineQueryRepository = new Mock<ITranscriptionEngineQueryRepository>();

			MockIUserCommandRepository
				.Setup(p => p.CreateUserAsync(It.IsAny<CreateUserDto>()))
				.ReturnsAsync(AFFECTED_ROWS);
			MockIUserCommandRepository
				.Setup(p => p.CreateUserRoleAsync(It.IsAny<CreateUserRoleDto>()));
			MockIUserCommandRepository
				.Setup(p => p.EditUserRoleAsync(It.IsAny<EditUserRoleDto>()));
			MockIUserCommandRepository
				.Setup(p => p.UpdatePasswordAsync(It.IsAny<UpdatePasswordDto>()));
			MockIUserCommandRepository
				.Setup(p => p.UpdateUserStatusAsync(It.IsAny<UpdateUserStatusDto>()))
				.ReturnsAsync(AFFECTED_ROWS);
			mockIUserQueryRepository
				.Setup(p => p.GetUserByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(usersList.FirstOrDefault());
			mockIUserQueryRepository
				.Setup(p => p.GetAllUsersAsync())
				.ReturnsAsync(usersList);
			mockIUserQueryRepository
				.Setup(p => p.GetUserByEmailActivationIdAsync(It.IsAny<string>()))
				.ReturnsAsync(usersList.FirstOrDefault());
			mockIUserQueryRepository
				.Setup(p => p.GetUserByEmailAsync(It.IsAny<string>()))
				.Returns<string>(async (email) =>
				{
					await Task.Yield();
					if (email == USER_SUCCESS_EMAIL)
					{
						return usersList.FirstOrDefault();
					}

					return null;
				});
			mockIUserQueryRepository
				.Setup(p => p.CheckTemporalPasswordAsync(It.IsAny<int>(), It.IsAny<string>()))
				.Returns<int, string>(async (userId, temporalPassword) =>
				{
					await Task.Yield();
					if (temporalPassword.Equals(USER_TEMPORARY_PASSWORD))
					{
						return true;
					}

					return false;
				});
			mockIRoleQueryRepository
				.Setup(p => p.GetAllRolesAsync())
				.ReturnsAsync(rolesList);
			mockIEmailDomain
				.Setup(p => p.SendUserRegisterEmail(It.IsAny<UserModel>()));
			mockIEmailDomain
				.Setup(p => p.SendPasswordRecoveryEmail(It.IsAny<UserModel>(), It.IsAny<string>()));
			mockIUserLoginHistoryQueryRepository
				.Setup(p => p.GetLastUserLoginStoryByUserIdAsync(It.IsAny<int>()))
				.ReturnsAsync(userLoginHistoryModel);
			MockIAuthenticationDataProvider
				.Setup(p => p.GetUserGuid(It.IsAny<string>()))
				.ReturnsAsync(STANDARD_USER_GUID);
			MockIAuthenticationDataProvider
				.Setup(p => p.CheckUserAPICredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			mockIPasswordHasher
				.Setup(p => p.Check(It.IsAny<string>(), It.IsAny<string>()))
				.Returns<string, string>((hashed, current) =>
				{
					if (current.Equals(USER_CURRENT_PASSWORD_SUCCESS))
					{
						return true;
					}

					return false;
				});

			MockIAuthenticationDataProvider
				.Setup(p => p.GetUserGuid(It.IsAny<string>()))
				.ReturnsAsync(STANDARD_USER_GUID);

			mockIConfiguration
				.SetupGet(x => x[It.Is<string>(s => s == "TranscriptionApiURL")])
				.Returns(STANDARD_USER_URL);

			mockITranscriptionEngineQueryRepository
				.Setup(x => x.GetAllTranscriptionEnginesAsync())
				.ReturnsAsync(transcriptionEngineList);


			Mock<IUserContext> mockIUserContext = new Mock<IUserContext>();

			mockIUserContext.SetupGet(p => p.UserCommandRepository)
				.Returns(MockIUserCommandRepository.Object);

			mockIUserContext.SetupGet(p => p.UserQueryRepository)
				.Returns(mockIUserQueryRepository.Object);

			mockIUserContext.SetupGet(p => p.RoleQueryRepository)
				.Returns(mockIRoleQueryRepository.Object);

			mockIUserContext.SetupGet(p => p.EmailDomain)
				.Returns(mockIEmailDomain.Object);

			mockIUserContext.SetupGet(p => p.PasswordHasher)
				.Returns(mockIPasswordHasher.Object);

			mockIUserContext.SetupGet(p => p.UserLoginHistoryQueryRepository)
				.Returns(mockIUserLoginHistoryQueryRepository.Object);

			mockIUserContext.SetupGet(p => p.AuthenticationDataProvider)
				.Returns(MockIAuthenticationDataProvider.Object);

			mockIUserContext.SetupGet(p => p.Configuration)
				.Returns(mockIConfiguration.Object);

			mockIUserContext.SetupGet(p => p.TranscriptionEngineQueryRepository)
				.Returns(mockITranscriptionEngineQueryRepository.Object);

			Mock<ILogger<UserDomain>> mockILogger = new Mock<ILogger<UserDomain>>();
			Mock<IHubContext<AdminNotificationHub>> mockIHubContext = new Mock<IHubContext<AdminNotificationHub>>();

			InstanceToTest = new UserDomain(mockIUserContext.Object, mockILogger.Object, mockIHubContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task CreateUserAsync_AdminSuccessTestAsync()
		{
			CreateUserDto createUserDto = new CreateUserDto
			{
				RoleId = (int)RoleEnum.Administrator,
				Email = "fakeadminemail@fake.com",
				FirstName = "Fake first name",
				LastName = "Fake last name"
			};

			var result = await InstanceToTest.CreateUserAsync(createUserDto);
			MockIAuthenticationDataProvider.Verify(m => m.GetUserGuid(It.IsAny<string>()), Times.Never);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task CreateUserAsync_StandardSuccessTestAsync()
		{
			CreateUserDto createUserDto = new CreateUserDto
			{
				RoleId = (int)RoleEnum.Standard,
				Email = "fakestandardemail@fake.com",
				FirstName = "Fake first name",
				LastName = "Fake last name"
			};

			var result = await InstanceToTest.CreateUserAsync(createUserDto);
			MockIAuthenticationDataProvider.Verify(m => m.GetUserGuid(It.IsAny<string>()), Times.AtLeastOnce);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetUserByIdAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetUserByIdAsync(1);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetAllUsersAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetAllUsersAsync();
			Assert.NotNull(result);
			Assert.True(result.First().LastLogin != null);
		}

		[Fact]
		public async Task GetAllRolesAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetAllRolesAsync();
			Assert.NotNull(result);
		}

		[Fact]
		public async Task EditUserAsync_UpdateEmailActivationIdSuccessTestAsync()
		{

			EditUserDto editUserDto = new EditUserDto
			{
				Id = 1,
				LastModifiedBy = 1,
				Email = "newfakeemail@fake.com",
				EmailActivationId = Guid.NewGuid().ToString()
			};

			var userBeforeUpdateModel = await InstanceToTest.GetUserByIdAsync(editUserDto.Id);

			var updateResult = await InstanceToTest.EditUserAsync(editUserDto);

			Assert.NotNull(updateResult);
			Assert.Equal(updateResult.Roles.FirstOrDefault()?.RoleId, userBeforeUpdateModel.Roles.FirstOrDefault()?.RoleId);
			Assert.NotEqual(updateResult.EmailActivationId, editUserDto.EmailActivationId);
			MockIUserCommandRepository.Verify(m => m.EditUserRoleAsync(It.IsAny<EditUserRoleDto>()), Times.Never);
		}

		[Fact]
		public async Task EditUserAsync_SuccessTestAsync()
		{

			EditUserDto editUserDto = new EditUserDto
			{
				Id = 1,
				LastModifiedBy = 1,
				Email = "fakeemail@fake.com",
			};

			var userBeforeUpdateModel = await InstanceToTest.GetUserByIdAsync(editUserDto.Id);

			var updateResult = await InstanceToTest.EditUserAsync(editUserDto);

			Assert.NotNull(updateResult);
			Assert.Equal(updateResult.Roles.FirstOrDefault()?.RoleId, userBeforeUpdateModel.Roles.FirstOrDefault()?.RoleId);
			Assert.Equal(updateResult.EmailActivationId, userBeforeUpdateModel.EmailActivationId);
			MockIUserCommandRepository.Verify(m => m.EditUserRoleAsync(It.IsAny<EditUserRoleDto>()), Times.Never);
		}

		[Fact]
		public async Task SetPasswordAsync_SuccessTestAsync()
		{
			SetPasswordDto setPasswordDto = new SetPasswordDto
			{
				Password = "fake password",
				ConfirmPassword = "confirm fake password",
				EmailActivationId = Guid.NewGuid().ToString()
			};

			var result = await InstanceToTest.SetPasswordAsync(setPasswordDto);
			Assert.True(result);
		}

		[Fact]
		public async Task ResetPasswordAsync_SuccessTestAsync()
		{
			await InstanceToTest.ResetPasswordAsync(USER_SUCCESS_EMAIL);
			Assert.True(true);
		}

		[Fact]
		public async Task CheckUserAPICredentialsAsync_SuccessTestAsync()
		{
			CheckUserApiDto dto = new CheckUserApiDto()
			{
				ApiUrl = "fake url",
				ApiGuid = Guid.NewGuid().ToString(),
			};

			var result = await InstanceToTest.CheckUserAPICredentialsAsync(dto);

			Assert.True(result);
		}

		[Fact]
		public async Task ChangePasswordAsync_SuccessTestAsync()
		{
			ChangePasswordDto dto = new ChangePasswordDto()
			{
				CurrentPassword = USER_CURRENT_PASSWORD_SUCCESS,
				NewPassword = "fake password"
			};
			await InstanceToTest.ChangePasswordAsync(dto);
			Assert.True(true);
		}

		[Fact]
		public async Task ChangePasswordAsync_FailedInvalidCurrentPasswordTestAsync()
		{
			ChangePasswordDto dto = new ChangePasswordDto()
			{
				CurrentPassword = "fake error current password"
			};
			await Assert.ThrowsAsync<ApplicationException>(() => InstanceToTest.ChangePasswordAsync(dto));
		}
		[Fact]
		public async Task GetNewUserGuidAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetNewUserGuidAsync("fake url");

			Assert.Equal(STANDARD_USER_GUID, result);
		}

		[Fact]
		public async Task PasswordRecoveryAsync_SuccessTestAsync()
		{
			PasswordRecoveryDto dto = new PasswordRecoveryDto()
			{
				Email = USER_SUCCESS_EMAIL,
				TemporalPassword = USER_TEMPORARY_PASSWORD,
				Password = "fake password",
			};

			await InstanceToTest.PasswordRecoveryAsync(dto);

			Assert.True(true);
		}

		[Fact]
		public async Task PasswordRecoveryAsync_FailedEmailNotExistsTestAsync()
		{
			PasswordRecoveryDto dto = new PasswordRecoveryDto()
			{
				Email = "fake error email",
			};

			await Assert.ThrowsAsync<ApplicationException>(() => InstanceToTest.PasswordRecoveryAsync(dto));
		}

		[Fact]
		public async Task PasswordRecoveryAsync_FailedIncorrectTemporayPasswordTestAsync()
		{
			PasswordRecoveryDto dto = new PasswordRecoveryDto()
			{
				Email = USER_SUCCESS_EMAIL,
				TemporalPassword = "fake error temporary password",
			};

			await Assert.ThrowsAsync<ApplicationException>(() => InstanceToTest.PasswordRecoveryAsync(dto));
		}

		[Fact]
		public async Task GetAllTranscriptionEnginesAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetAllTranscriptionEnginesAsync();
			Assert.NotNull(result);
		}


		#endregion
	}
}
