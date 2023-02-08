using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
	[Trait("UnitTest", "PermissionDomain")]
	public class PermissionDomainTests
	{
		#region [Setup]

		private PermissionDomain InstanceToTest { get; set; }

		public PermissionDomainTests()
		{
			Mock<IRoleQueryRepository> mockIRoleQueryRepository = new Mock<IRoleQueryRepository>();
			Mock<IMemoryCache> mockIMemoryCache = new Mock<IMemoryCache>();
			Mock<MemoryCacheConfiguration> mockMemoryCacheConfiguration = new Mock<MemoryCacheConfiguration>();
			Mock<ITranscriptQueryRepository> mockITranscriptQueryRepository = new Mock<ITranscriptQueryRepository>();

			mockITranscriptQueryRepository
				.Setup(p => p.ChekUserTranscriptAccessAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(true);

			InstanceToTest = new PermissionDomain(
				mockIRoleQueryRepository.Object,
				mockIMemoryCache.Object,
				mockMemoryCacheConfiguration.Object,
				mockITranscriptQueryRepository.Object
			);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task CheckTranscriptPermissionAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.CheckTranscriptPermissionAsync(1, 1);
			Assert.True(result);
		}

		#endregion
	}
}
