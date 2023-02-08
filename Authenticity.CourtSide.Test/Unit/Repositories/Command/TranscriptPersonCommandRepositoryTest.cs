using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
    [Trait("UnitTest", "TranscriptPersonCommandRepository")]
    public class TranscriptPersonCommandRepositoryTest
    {
        #region [Setup]

        private const int AFFECTED_ROWS = 1;
        private TranscriptPersonCommandRepository InstanceToTest { get; set; }
        private Mock<IDapperBase<TranscriptPerson>> MockDapperContext { get; set; }

        public TranscriptPersonCommandRepositoryTest()
        {
            MockDapperContext = new Mock<IDapperBase<TranscriptPerson>>();


            MockDapperContext
                .Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(AFFECTED_ROWS);
            
            InstanceToTest = new TranscriptPersonCommandRepository(MockDapperContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task DeleteAllPeopleByTranscriptIdAsync_SuccessTestAsync()
        {
            var result = await InstanceToTest.DeleteAllPeopleByTranscriptIdAsync(1);
            Assert.Equal(result, AFFECTED_ROWS);
        }
        
        #endregion
    }
}
