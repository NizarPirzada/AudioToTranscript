using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "TranscriptFileCommandRepository")]
    public class TranscriptFileCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private TranscriptFileCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<TranscriptFile>> MockDapperContext { get; set; }

        public TranscriptFileCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<TranscriptFile>>();


            MockDapperContext
                .Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);
            
            InstanceToTest = new TranscriptFileCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task DeleteAllFilesByTranscriptIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.DeleteAllFilesByTranscriptIdAsync(1);
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        #endregion
    }
}
