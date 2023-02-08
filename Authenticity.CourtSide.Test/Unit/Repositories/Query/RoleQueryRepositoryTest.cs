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
    [Trait("UnitTest", "RoleQueryRepository")]
    public class RoleQueryRepositoryTest
    {

        #region [Setup]

        private RoleQueryRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<RoleModel>> MockDapperContext { get; set; }

        public RoleQueryRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<RoleModel>>();

            List<RoleModel> rolesList = new List<RoleModel>
            {
                new RoleModel
                {
                    RoleId = 1
                }
            };


            MockDapperContext
                .Setup(p => p.GetAllAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(rolesList);

            InstanceToTest = new RoleQueryRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task GetAllRolesAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.GetAllRolesAsync();
            Assert.NotNull(result);
        }

        #endregion

    }
}
