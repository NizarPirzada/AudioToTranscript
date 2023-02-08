using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "TranscriptJobCommandRepository")]
    public class TranscriptJobCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private TranscriptJobCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<TranscriptJob>> MockDapperContext { get; set; }

        public TranscriptJobCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<TranscriptJob>>();


            MockDapperContext
                .Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockDapperContext
	            .Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
	            .ReturnsAsync(AFFECTED_ROWS);

            MockDapperContext
	            .Setup(p => p.EditAsync(It.IsAny<string>(), It.IsAny<object>()))
	            .ReturnsAsync(AFFECTED_ROWS);

            InstanceToTest = new TranscriptJobCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task DeleteAllJobsByTranscriptIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.DeleteAllJobsByTranscriptIdAsync(1);
            Assert.Equal(result, AFFECTED_ROWS);
        }

        [Fact]
        public async Task CreateJobAsync_SuccessTestAsync()
        {
	        var result = await InstanceToTest.CreateJobAsync(new TranscriptJob());
	        Assert.Equal(result, AFFECTED_ROWS);
        }

        [Fact]
        public async Task UpdateJobAsync_SuccessTestAsync()
        {
	        var result = await InstanceToTest.UpdateJobAsync(new TranscriptJob());
	        Assert.True(result);
        }

        #endregion
    }
}
