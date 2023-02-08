using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Query.Implementation;
using Moq;
using NSerio.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Query
{
	[Trait("UnitTest", "TranscriptionEngineQueryRepository")]
	public class TranscriptionEngineQueryRepositoryTest
	{

		#region [Setup]

		private TranscriptionEngineQueryRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<TranscriptionEngine>> MockDapperContext { get; set; }

		public TranscriptionEngineQueryRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<TranscriptionEngine>>();


			List<TranscriptionEngine> transcriptionEngineList = new List<TranscriptionEngine>
			{
				new TranscriptionEngine
				{
					Id = 1,
					Name = "Fake engine",
					Code = "Fake engine code"
				}
			};

			MockDapperContext
				.Setup(p => p.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(transcriptionEngineList.FirstOrDefault());

			MockDapperContext
				.Setup(p => p.GetAllAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(transcriptionEngineList);

			InstanceToTest = new TranscriptionEngineQueryRepository(MockDapperContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task GetAllTranscriptionEnginesAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetAllTranscriptionEnginesAsync();
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetAllLanguagesByTypeAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetTranscriptionEngineByIdAsync(1);
			Assert.NotNull(result);
		}

		#endregion
	}
}
