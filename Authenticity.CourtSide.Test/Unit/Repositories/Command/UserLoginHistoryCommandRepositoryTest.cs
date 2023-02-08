using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "UserLoginHistoryCommandRepository")]
    public class UserLoginHistoryCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private UserLoginHistoryCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<UserLoginHistoryDto>> MockDapperContext { get; set; }

        public UserLoginHistoryCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<UserLoginHistoryDto>>();


            MockDapperContext
                .Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);

            InstanceToTest = new UserLoginHistoryCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task CreateUserLoginHistoryAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.CreateUserLoginHistoryAsync(new UserLoginHistoryDto());
            Assert.Equal(result, AFFECTED_ROWS);
        }

        #endregion
    }
}
