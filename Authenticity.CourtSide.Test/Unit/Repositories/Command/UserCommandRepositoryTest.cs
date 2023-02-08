using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "UserCommandRepository")]
    public class UserCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private UserCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<UserModel>> MockDapperContext { get; set; }

        public UserCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<UserModel>>();


            MockDapperContext
                .Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);

             MockDapperContext
                .Setup(p => p.EditAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);

            InstanceToTest = new UserCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task CreateUserAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.CreateUserAsync(new CreateUserDto());
            Assert.Equal(result, AFFECTED_ROWS);
        }

        [Fact]
        public async Task CreateUserRoleAsync_SuccessTestAsync()
        {
            await InstanceToTest.CreateUserRoleAsync(new CreateUserRoleDto());
            Assert.True(true);
        }

        [Fact]
        public async Task EditUserAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.EditUserAsync(new EditUserDto());
            Assert.Equal(result, AFFECTED_ROWS);
        }
        [Fact]
        public async Task EditUserRoleAsync_SuccessTestAsync()
        {
            await InstanceToTest.EditUserRoleAsync(new EditUserRoleDto());
            Assert.True(true);
        }

        [Fact]
        public async Task UpdatePasswordAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.UpdatePasswordAsync(new UpdatePasswordDto());
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        [Fact]
        public async Task UpdateUserStatusAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.UpdateUserStatusAsync(new UpdateUserStatusDto());
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        [Fact]
        public async Task SetTemporalPasswordAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.SetTemporalPasswordAsync(1, "fake temp password");
            Assert.Equal(result, AFFECTED_ROWS);
        }

        #endregion
    }
}
