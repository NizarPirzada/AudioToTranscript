using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Models.Transcription;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
	[Trait("UnitTest", "TranscriptionProvidersDomain")]
	public class TranscriptionProvidersDomainTest
	{
		#region [Setup]
		private static readonly string TRANSCRIPT_EXPORT_FILE_TEMPLATE_PATH = "fakePath";
		private static readonly int USER_ID_WITH_API_URL = 1;
		private static readonly int USER_ID_WITHOUT_API_URL = 2;
		private static readonly int AFFECTED_ROWS = 1;

		private static readonly IEnumerable<TranscriptJob> FULL_TRANSCRIPT_JOBS_LIST = new List<TranscriptJob>() {
			new TranscriptJob()
			{
				Id = 1,
				CreatedBy = USER_ID_WITH_API_URL
			},
			new TranscriptJob()
			{
				Id = 2,
				CreatedBy = USER_ID_WITHOUT_API_URL
			}
		};

		private static readonly UserModel USER_WITH_API_URL = new UserModel()
		{
			Id = 2,
			FirstName = string.Empty,
			LastName = string.Empty,
			ApiUrl = "Fake API URL",
			ApiGuid = "Fake API GUID"
		};

		private static readonly UserModel USER_WITHOUT_API_URL = new UserModel()
		{
			Id = 2,
			FirstName = string.Empty,
			LastName = string.Empty,
		};

		private static readonly IEnumerable<TranscriptFile> FULL_TRANSCRIPT_FILE_LIST = new List<TranscriptFile>() {
			new TranscriptFile()
			{
				Path = "Fake path file 1"
			},
			new TranscriptFile()
			{
				Path = "Fake path file 2"
			}
		};

		private TranscriptionProvidersDomain InstanceToTest { get; }

		public TranscriptionProvidersDomainTest()
		{

			Mock<ITranscriptJobQueryRepository> mockITranscriptJobQueryRepository = new Mock<ITranscriptJobQueryRepository>();
			mockITranscriptJobQueryRepository = new Mock<ITranscriptJobQueryRepository>();

			Mock<ITranscriptFileQueryRepository> mockITranscriptFileQueryRepository = new Mock<ITranscriptFileQueryRepository>();
			mockITranscriptFileQueryRepository = new Mock<ITranscriptFileQueryRepository>();

			Mock<ITranscriptJobCommandRepository> mockITranscriptJobCommandRepository = new Mock<ITranscriptJobCommandRepository>();
			mockITranscriptJobCommandRepository = new Mock<ITranscriptJobCommandRepository>();

			Mock<ITranscriptDialogCommandRepository> mockITranscriptDialogCommandRepository = new Mock<ITranscriptDialogCommandRepository>();
			mockITranscriptDialogCommandRepository = new Mock<ITranscriptDialogCommandRepository>();
			
			Mock<ITranscriptCommandRepository> mockITranscriptCommandRepository = new Mock<ITranscriptCommandRepository>();
			mockITranscriptCommandRepository = new Mock<ITranscriptCommandRepository>();
			
			Mock<ITranscriptionEngine> mockITranscriptionEngine = new Mock<ITranscriptionEngine>();
			mockITranscriptionEngine = new Mock<ITranscriptionEngine>();
			
			Mock<IFileProviderFactory> mockIFileProviderFactory = new Mock<IFileProviderFactory>();
			mockIFileProviderFactory = new Mock<IFileProviderFactory>();

			Mock<IUserQueryRepository> mockIUserQueryRepository = new Mock<IUserQueryRepository>();
			mockIUserQueryRepository = new Mock<IUserQueryRepository>();

			mockITranscriptJobQueryRepository
				.Setup(p => p.GetBatchOfJobsByStatusAsync(It.IsAny<TranscriptJobStatusEnum>()))
				.ReturnsAsync(FULL_TRANSCRIPT_JOBS_LIST);

			mockITranscriptFileQueryRepository
				.Setup(p => p.GetAllTranscriptFileByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(FULL_TRANSCRIPT_FILE_LIST);

			Mock<IFileProvider> mockIFileProvider = new Mock<IFileProvider>();

			mockIFileProvider
				.Setup(p => p.GetFileToLocalTempDirectoryAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(TRANSCRIPT_EXPORT_FILE_TEMPLATE_PATH);

			mockIFileProviderFactory
				.Setup(p => p.CreateAsync())
				.ReturnsAsync(mockIFileProvider.Object);

			mockIUserQueryRepository
				.Setup(p => p.GetUserByIdAsync(It.IsAny<int>()))
				.Returns<int>(async (userId) =>
				{
					await Task.Yield();
					if (userId == USER_ID_WITH_API_URL)
					{
						return USER_WITH_API_URL;
					}
					if (userId == USER_ID_WITHOUT_API_URL)
					{
						return USER_WITHOUT_API_URL;
					}

					return null;
				});

			mockITranscriptionEngine
				.Setup(p => p.CreateTranscriptRequestAsync(It.IsAny<TranscriptRequestModel>()))
				.ReturnsAsync(Guid.NewGuid().ToString());

			mockITranscriptionEngine
				.Setup(p => p.CheckTranscriptResponseAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Guid.NewGuid().ToString());

			mockITranscriptJobCommandRepository
				.Setup(p => p.UpdateJobAsync(It.IsAny<TranscriptJob>()))
				.ReturnsAsync(true);

			mockITranscriptDialogCommandRepository
				.Setup(p => p.CreateDialogAsync(It.IsAny<TranscriptDialog>()))
				.ReturnsAsync(AFFECTED_ROWS);

			mockITranscriptCommandRepository
				.Setup(p => p.UpdateTranscriptTextAsync(It.IsAny<int>(), It.IsAny<string>()))
				.ReturnsAsync(AFFECTED_ROWS);


			Mock<ITranscriptionProvidersDomainContext> mockITranscriptionProvidersDomainContext = new Mock<ITranscriptionProvidersDomainContext>();

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptJobQueryRepository)
				.Returns(mockITranscriptJobQueryRepository.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptFileQueryRepository)
				.Returns(mockITranscriptFileQueryRepository.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptJobCommandRepository)
				.Returns(mockITranscriptJobCommandRepository.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptDialogCommandRepository)
				.Returns(mockITranscriptDialogCommandRepository.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptCommandRepository)
				.Returns(mockITranscriptCommandRepository.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.TranscriptionEngine)
				.Returns(mockITranscriptionEngine.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.FileProviderFactory)
				.Returns(mockIFileProviderFactory.Object);

			mockITranscriptionProvidersDomainContext.SetupGet(p => p.UserQueryRepository)
				.Returns(mockIUserQueryRepository.Object);

			InstanceToTest = new TranscriptionProvidersDomain(mockITranscriptionProvidersDomainContext.Object);
		}

		#endregion

		#region [Tests]

		[Fact]
		public async Task SendNewJobsToTranscriptionEngineAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.SendNewJobsToTranscriptionEngineAsync();
			Assert.True(result >= AFFECTED_ROWS);
		}

		[Fact]
		public async Task CheckPendingJobsFromTranscriptionEngineAsync_SuccessTestAsync()
		{
			var result = await InstanceToTest.CheckPendingJobsFromTranscriptionEngineAsync();
			Assert.True(result >= AFFECTED_ROWS);
		}

		#endregion
	}
}
