using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
    [Trait("UnitTest", "UserLoginHistoryQueryRepository")]
    public class UserLoginHistoryQueryRepositoryTest
    {

        #region [Setup]

        private const int EXISTENT_USER_ID = 1;

        private UserLoginHistoryQueryRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<UserLoginHistoryDto>> MockDapperContext { get; set; }

        public UserLoginHistoryQueryRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<UserLoginHistoryDto>>();
            UserLoginHistoryDto userLoginHistoryModel = new UserLoginHistoryDto
            {
                Id = 1,
                LoginTime = DateTime.Now,
                UserId = EXISTENT_USER_ID
            };

            MockDapperContext
                .Setup(p => p.GetAsync<UserLoginHistoryDto>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(userLoginHistoryModel);

            InstanceToTest = new UserLoginHistoryQueryRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task GetLastUserLoginStoryByUserId_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetLastUserLoginStoryByUserIdAsync(EXISTENT_USER_ID);
            Assert.NotNull(result);
        }

        #endregion

    }
}
