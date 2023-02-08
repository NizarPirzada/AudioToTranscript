using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "TranscriptCommandRepository")]
    public class TranscriptCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private TranscriptCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<Transcript>> MockDapperContext { get; set; }

        public TranscriptCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<Transcript>>();


            MockDapperContext
                .Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);
            
            InstanceToTest = new TranscriptCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task DeleteAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.DeleteAsync(1);
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        #endregion
    }
}
