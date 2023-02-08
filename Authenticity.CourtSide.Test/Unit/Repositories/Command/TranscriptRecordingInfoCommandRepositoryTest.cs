using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
	[Trait("UnitTest", "TranscriptRecordingInfoCommandRepository")]
	public class TranscriptRecordingInfoCommandRepositoryTest
	{
		#region [Setup]

		private const int AFFECTED_ROWS = 1;

		private TranscriptRecordingInfoCommandRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<TranscriptRecordingInfoModel>> MockDapperContext { get; set; }

		public TranscriptRecordingInfoCommandRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<TranscriptRecordingInfoModel>>();

			MockDapperContext
				.Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(AFFECTED_ROWS);

			MockDapperContext
				.Setup(p => p.EditAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(AFFECTED_ROWS);

			InstanceToTest = new TranscriptRecordingInfoCommandRepository(MockDapperContext.Object);

		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task CreateTranscriptRecordingInfoAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.CreateTranscriptRecordingInfoAsync(new TranscriptRecordingInfoModel());
			Assert.Equal(result, AFFECTED_ROWS);
		}

		[Fact]
		public async Task UpdateTranscriptRecordingInfoAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.UpdateTranscriptRecordingInfoAsync(new TranscriptRecordingInfoModel());
			Assert.Equal(result, AFFECTED_ROWS);
		}

		#endregion


	}
}
