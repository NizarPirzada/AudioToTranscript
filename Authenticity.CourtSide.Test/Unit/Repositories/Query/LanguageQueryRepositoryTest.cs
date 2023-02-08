using Authenticity.CourtSide.Core.Enums;
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
	[Trait("UnitTest", "LanguageQueryRepository")]
	public class LanguageQueryRepositoryTest
	{

		#region [Setup]

		private LanguageQueryRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<ApiLanguage>> MockDapperContext { get; set; }

		public LanguageQueryRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<ApiLanguage>>();


			List<ApiLanguage> apiLanguageList = new List<ApiLanguage>
			{
				new ApiLanguage
				{
					ApiLanguageId = 1,
					LanguageType = LanguageTypeEnum.Transcription,
					Name = "English",
					ApiCode = "en",
					GenericCode = "en",
					CreatedBy = 1,
					CreatedOn = new System.DateTime()
				}
			};

			MockDapperContext
				.Setup(p => p.GetByIdAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(apiLanguageList.FirstOrDefault());

			MockDapperContext
				.Setup(p => p.GetAllAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(apiLanguageList);

			InstanceToTest = new LanguageQueryRepository(MockDapperContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task GetLanguageByIdAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetLanguageByIdAsync(1);
			Assert.NotNull(result);
		}

		[Fact]
		public async Task GetAllLanguagesByTypeAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.GetAllLanguagesByTypeAsync(LanguageTypeEnum.Transcription);
			Assert.NotNull(result);
		}

		#endregion
	}
}
