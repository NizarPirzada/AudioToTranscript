using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
	[Trait("UnitTest", "TranscriptJobQueryRepository")]
	public class TranscriptJobQueryRepositoryTest
	{

		#region [Setup]

		private TranscriptJobQueryRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<TranscriptJob>> MockDapperContext { get; set; }

		public TranscriptJobQueryRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<TranscriptJob>>();

			List<TranscriptJob> jobsList = new List<TranscriptJob>
			{
				new TranscriptJob
				{
					Id = 1,
					TranscriptId = 1,
					Status = TranscriptJobStatusEnum.Created,
				},
				new TranscriptJob
				{
					Id = 1,
					TranscriptId = 1,
					Status = TranscriptJobStatusEnum.Completed,
				}
			};

			MockDapperContext
				.Setup(p => p.GetAllAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(jobsList);

			InstanceToTest = new TranscriptJobQueryRepository(MockDapperContext.Object);
		}


		#endregion

		#region [Tests]

		[Fact]
		public async Task GetBatchOfJobsByStatusAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetBatchOfJobsByStatusAsync(TranscriptJobStatusEnum.Created);
			Assert.NotNull(result);
		}


		#endregion
	}
}
