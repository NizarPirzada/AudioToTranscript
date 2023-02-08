using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
	[Trait("UnitTest", "TranscriptRecordingInfoQueryRepository")]
	public class TranscriptRecordingInfoQueryRepositoryTest
	{
		#region [Setup]

		private TranscriptRecordingInfoQueryRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<TranscriptRecordingInfoModel>> MockDapperContext { get; set; }


		public TranscriptRecordingInfoQueryRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<TranscriptRecordingInfoModel>>();

			TranscriptRecordingInfoModel recordingInfo = new TranscriptRecordingInfoModel
			{
				TranscriptRecordingInfoId = 1,

			};


			MockDapperContext
				.Setup(p => p.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(recordingInfo);

			InstanceToTest = new TranscriptRecordingInfoQueryRepository(MockDapperContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task GetTranscriptRecordingInfoByIdAsync_TestAsync()
		{
			var result = await InstanceToTest.GetTranscriptRecordingInfoByIdAsync(1);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetTranscriptRecordingInfoByTranscriptIdAsync_TestAsync()
		{
			var result = await InstanceToTest.GetTranscriptRecordingInfoByTranscriptIdAsync(1);
			Assert.NotNull(result);
		}


		#endregion
	}
}
