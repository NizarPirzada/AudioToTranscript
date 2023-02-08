using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
    public class AuthenticationDomain : IAuthenticationDomain
    {
        private const string INCORRECT_CREDENTIALS = "Login failed. Please check your credentials";
        private const string DEACTIVE_ACCOUNT = "Account has been deactivated. Please contact an administrator";
        private const string PENDING_ACCOUNT = "Account activation is pending. Check your email inbox and follow the instructions";

        #region [Public]

        private IUserQueryRepository UserQueryRepository { get; }
        private IPasswordHasher PasswordsHasher { get; }
        private IUserLoginHistoryCommandRepository UserLoginHistoryCommandRepository { get; }

        public AuthenticationDomain(IUserQueryRepository userQueryRepository, IPasswordHasher passwordsHasher, IUserLoginHistoryCommandRepository userLoginHistoryCommandRepository)
        {
            UserQueryRepository = userQueryRepository;
            PasswordsHasher = passwordsHasher;
            UserLoginHistoryCommandRepository = userLoginHistoryCommandRepository;
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto, int expirationTokenTime)
        {
            UserModel user = await UserQueryRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new InvalidOperationException(INCORRECT_CREDENTIALS);
            }

            switch (user.Status)
            {
                case Enums.UserStatusEnum.Deactivated:
                    throw new InvalidDataException(DEACTIVE_ACCOUNT);
                case Enums.UserStatusEnum.Pending:
                    throw new InvalidDataException(PENDING_ACCOUNT);
            }

            //Validate password
            bool passwordVerified = PasswordsHasher.Check(user.Password, loginDto.Password);

            if (!passwordVerified)
            {
                throw new InvalidDataException(INCORRECT_CREDENTIALS);
            }

            //Generate Token
            LoginResponseDto response = TokenGenerator.GenerateTokenJwt(user, expirationTokenTime);

            // Inser LogHistory
            UserLoginHistoryDto userLoginHistoryModel = new UserLoginHistoryDto
            {
                UserId = user.Id
            };
            await UserLoginHistoryCommandRepository.CreateUserLoginHistoryAsync(userLoginHistoryModel);

            response.User = user;

            return response;
        }

        #endregion
    }
}
