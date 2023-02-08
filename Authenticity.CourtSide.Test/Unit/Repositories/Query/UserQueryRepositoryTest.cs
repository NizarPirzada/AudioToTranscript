using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
    [Trait("UnitTest", "LoginQueryRepository")]
    public class UserQueryRepositoryTest
    {

        #region [Setup]

        private const string EXISTENT_USER_EMAIL = "fakeEmail@courtside.com";
        private const int EXISTENT_USER_ID = 1;

        private UserQueryRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<UserModel>> MockDapperUser { get; set; }

        public UserQueryRepositoryTest()
        {
            MockDapperUser = new Mock<IDapperBase<UserModel>>();
            List<UserModel> usersList = new List<UserModel>
            {
                new UserModel
                {
                    Id = EXISTENT_USER_ID,
                    FirstName = "Fake First Name",
                    LastName = "Fake Last Name",
                    Email = EXISTENT_USER_EMAIL
                }
            };
            MockDapperUser
                .Setup(p => p.GetByIdWithRelationsAsync(It.IsAny<string>(),
                It.IsAny<Func<UserModel, RoleModel, IDictionary<int, UserModel>, UserModel>>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
                .ReturnsAsync(usersList);

            MockDapperUser
                .Setup(p => p.QueryAllWithRelationsAsync(It.IsAny<string>(),
                It.IsAny<Func<UserModel, RoleModel, IDictionary<int, UserModel>, UserModel>>(),
                It.IsAny<string>(),
                It.IsAny<object>())).ReturnsAsync(usersList);

            MockDapperUser
                .Setup(p => p.GetAsync<bool>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(true);

            InstanceToTest = new UserQueryRepository(MockDapperUser.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task GetUserByEmailAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetUserByEmailAsync(EXISTENT_USER_EMAIL);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetUserByIdAsync(EXISTENT_USER_ID);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetAllUsersAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserByEmailActivationIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetUserByEmailActivationIdAsync(Guid.NewGuid().ToString());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CheckTemporalPasswordAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.CheckTemporalPasswordAsync(1, "fake temp password");
            Assert.True(result);
        }


        #endregion

    }
}
