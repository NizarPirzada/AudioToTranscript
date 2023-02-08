using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Repositories.Command.Implementation;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Repositories.Command
{
	[Trait("UnitTest", "TranscriptDialogCommandRepository")]
	public class TranscriptDialogCommandRepositoryTest
	{
		#region [Setup]

		private const int AFFECTED_ROWS = 1;
		private TranscriptDialogCommandRepository InstanceToTest { get; set; }
		private Mock<IDapperBase<TranscriptDialog>> MockDapperContext { get; set; }
		private Mock<IDapperBase<ChangeSpeakerDto>> MockDapperMassiveOperations { get; set; }

		public TranscriptDialogCommandRepositoryTest()
		{
			MockDapperContext = new Mock<IDapperBase<TranscriptDialog>>();
			MockDapperMassiveOperations = new Mock<IDapperBase<ChangeSpeakerDto>>();

			MockDapperMassiveOperations
				.Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(AFFECTED_ROWS);

			MockDapperContext
				.Setup(p => p.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(AFFECTED_ROWS);

			MockDapperContext
				.Setup(p => p.SaveAsync(It.IsAny<string>(), It.IsAny<object>()))
				.ReturnsAsync(AFFECTED_ROWS);

			InstanceToTest = new TranscriptDialogCommandRepository(MockDapperContext.Object, MockDapperMassiveOperations.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task DeleteDialogsByTranscriptIdAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.DeleteDialogsByTranscriptIdAsync(1);
			Assert.Equal(result, AFFECTED_ROWS);
		}

		[Fact]
		public async Task UpdateAllSpeakersAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.UpdateAllSpeakersAsync(new ChangeSpeakerDto());
			Assert.Equal(result, AFFECTED_ROWS);
		}

		[Fact]
		public async Task UpdateSingleExaminationTagAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.UpdateSingleExaminationAsync(new TranscriptDialog());
			Assert.Equal(result, AFFECTED_ROWS);
		}


		[Fact]
		public async Task UpdateMassivelyExaminationTagAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.UpdateMassivelyExaminationTagAsync(new TranscriptDialog(), new TranscriptDialog());
			Assert.Equal(result, AFFECTED_ROWS);
		}



		#endregion
	}
}
