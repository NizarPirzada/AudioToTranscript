using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "CourtCommandRepository")]
    public class CourtCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private CourtCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<CourtModel>> MockDapperContext { get; set; }

        public CourtCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<CourtModel>>();


            MockDapperContext
                .Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);
            
            InstanceToTest = new CourtCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task DeleteCourtByTranscriptIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.DeleteCourtByTranscriptIdAsync(1);
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        #endregion
    }
}
