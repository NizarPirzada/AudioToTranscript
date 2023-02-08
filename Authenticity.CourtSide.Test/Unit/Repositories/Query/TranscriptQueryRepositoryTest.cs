using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
	[Trait("UnitTest", "TranscriptQueryRepository")]
	public class TranscriptQueryRepositoryTest
	{

		#region [Setup]

		private TranscriptQueryRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<Transcript>> MockDapperContext { get; set; }

		public TranscriptQueryRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<Transcript>>();

			MockDapperContext
				.Setup(p => p.GetAsync<bool>(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(true);

			InstanceToTest = new TranscriptQueryRepository(MockDapperContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task ChekUserTranscriptAccessAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.ChekUserTranscriptAccessAsync(1, 1);
			Assert.True(result);
		}

		#endregion
	}
}
