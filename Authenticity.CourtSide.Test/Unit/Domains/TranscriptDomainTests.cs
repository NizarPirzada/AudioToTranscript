using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Command;
using Authenticity.CourtSide.Core.Repositories.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Microsoft.AspNetCore.Http;
using Xunit;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
    [Trait("UnitTest", "TranscriptDomain")]
    public class TranscriptDomainTests
    {
        #region [Setup]

        private static readonly int USER_ID_WITHOUT_TRANSCRIPTS = 1;
        private static readonly int USER_ID_WITH_TRANSCRIPTS = 2;
        private static readonly int USER_ID_WITH_NEW_TRANSCRIPTS = 3;
        private static readonly int USER_ID_WITH_OLD_TRANSCRIPTS = 4;
        private static readonly int AFFECTED_ROWS = 1;
        private static readonly int PERSON_ID_WITH_ADDITIONAL_INFO = 1;
        private static readonly int PERSON_ID_WITHOUT_ADDITIONAL_INFO = 2;
        private static readonly int USER_ID_WITH_API_URL = 1;
        private static readonly int USER_ID_WITHOUT_API_URL = 2;
        private static readonly int TRANSCRIPT_ID_WITH_FILES = 1;
        private static readonly int TRANSCRIPT_ID_WITHOUT_FILES = 2;

        private static readonly string TRANSCRIPT_EXPORT_FILE_TEMPLATE_PATH = "fakePath";
        private static readonly IEnumerable<Transcript> EMPTY_TRANSCRIPT_LIST = new List<Transcript>();
        private static readonly IEnumerable<Transcript> FULL_TRANSCRIPT_LIST_WITH_EMPTY_OBJECTS = new List<Transcript>() {
            new Transcript(),
            new Transcript()
        };
        private static readonly IEnumerable<Transcript> FULL_TRANSCRIPT_LIST_WITHOUT_LASTMODIFIEDON = new List<Transcript>() {
            new Transcript(),
            new Transcript(){
                CreatedOn = DateTime.Now.AddHours(3)
            }
        };
        private static readonly IEnumerable<Transcript> FULL_TRANSCRIPT_LIST_WITH_LASTMODIFIEDON = new List<Transcript>() {
            new Transcript(){
                CreatedOn = DateTime.Now.AddHours(9),
                LastModifiedOn = DateTime.Now.AddHours(7)
            },
            new Transcript(){
                CreatedOn = DateTime.Now.AddHours(8),
                LastModifiedOn = DateTime.Now.AddHours(6)
            }
        };

        private static readonly IEnumerable<TranscriptPerson> TRANSCRIPT_PERSON_LIST = new List<TranscriptPerson>() {
            new TranscriptPerson(),
            new TranscriptPerson()
        };

        private static readonly IEnumerable<TranscriptDialog> FULL_TRANSCRIPT_DIALOG_LIST = new List<TranscriptDialog>() {
            new TranscriptDialog(){
                Transcription = "Fake transcript dialog speaker 1",
                PersonId = 1
            },
            new TranscriptDialog(){
                Transcription = "Fake transcript dialog speaker 2",
                PersonId = 2
            },
        };

        private static readonly TranscriptPerson TRANSCRIPT_PERSON_WITH_ADDITIONAL_INFO = new TranscriptPerson()
        {
            Id = 1,
            FirstName = string.Empty,
            LastName = string.Empty,
            AdditionalInfo = new PersonAdditionalInformation()
            {
                PersonAdditionalInformationId = 1
            }
        };

        private static readonly TranscriptPerson TRANSCRIPT_PERSON_WITHOUT_ADDITIONAL_INFO = new TranscriptPerson()
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

        private TranscriptDomain InstanceToTest { get; }
        private Mock<IPersonAdditionalInfoCommandRepository> MockIPersonAdditionalInfoCommandRepository { get; set; }
        private Mock<ITranscriptFileCommandRepository> MockITranscriptFileCommandRepository { get; }
        private Mock<ITranscriptFileQueryRepository> MockITranscriptFileQueryRepository { get; }
        private Mock<ITranscriptDialogCommandRepository> MockTranscriptDialogCommandRepository { get; }
        private Mock<ITranscriptPersonCommandRepository> MockITranscriptPersonCommandRepository { get; }
        private Mock<ICourtCommandRepository> MockICourtCommandRepository { get; }
        private Mock<ITranscriptJobCommandRepository> MockITranscriptJobCommandRepository { get; }
        private Mock<ITranscriptCommandRepository> MockITranscriptCommandRepository { get; }

        public TranscriptDomainTests()
        {
            Mock<ITranscriptQueryRepository> mockITranscriptQueryRepository = new Mock<ITranscriptQueryRepository>();
            MockITranscriptCommandRepository = new Mock<ITranscriptCommandRepository>();

            Mock<ITranscriptPersonQueryRepository> mockITranscriptPersonQueryRepository = new Mock<ITranscriptPersonQueryRepository>();
            MockITranscriptPersonCommandRepository = new Mock<ITranscriptPersonCommandRepository>();

            MockITranscriptFileQueryRepository = new Mock<ITranscriptFileQueryRepository>();
            MockITranscriptFileCommandRepository = new Mock<ITranscriptFileCommandRepository>();

            MockITranscriptJobCommandRepository = new Mock<ITranscriptJobCommandRepository>();

            Mock<ITranscriptDialogQueryRepository> mockITranscriptDialogQueryRepository = new Mock<ITranscriptDialogQueryRepository>();
            MockTranscriptDialogCommandRepository = new Mock<ITranscriptDialogCommandRepository>();

            MockICourtCommandRepository = new Mock<ICourtCommandRepository>();

            Mock<IFileProvider> mockIFileProvider = new Mock<IFileProvider>();

            Mock<ICourtQueryRepository> mockCourtQueryRepository = new Mock<ICourtQueryRepository>();
            Mock<ICourtCommandRepository> mockCourtCommandRepository = new Mock<ICourtCommandRepository>();
            Mock<IUserQueryRepository> mockIUserQueryRepository = new Mock<IUserQueryRepository>();
            
            MockIPersonAdditionalInfoCommandRepository = new Mock<IPersonAdditionalInfoCommandRepository>();

            Mock<IFileProviderFactory> mockIFileProviderFactory = new Mock<IFileProviderFactory>();

            Mock<ILanguageQueryRepository> mockILanguageQueryRepository = new Mock<ILanguageQueryRepository>();

            mockITranscriptPersonQueryRepository
	            .Setup(p => p.GetAllTranscriptPersonsByTranscriptIdAsync(It.IsAny<int>()))
	            .ReturnsAsync(TRANSCRIPT_PERSON_LIST);

            mockITranscriptQueryRepository
                    .Setup(p => p.GetAllTranscriptsByUserAsync(USER_ID_WITHOUT_TRANSCRIPTS))
                    .ReturnsAsync(EMPTY_TRANSCRIPT_LIST);
            mockITranscriptQueryRepository
                .Setup(p => p.GetAllTranscriptsByUserAsync(USER_ID_WITH_TRANSCRIPTS))
                .ReturnsAsync(FULL_TRANSCRIPT_LIST_WITH_EMPTY_OBJECTS);
            mockITranscriptQueryRepository
                .Setup(p => p.GetAllTranscriptsByUserAsync(USER_ID_WITH_NEW_TRANSCRIPTS))
                .ReturnsAsync(FULL_TRANSCRIPT_LIST_WITHOUT_LASTMODIFIEDON);
            mockITranscriptQueryRepository
                .Setup(p => p.GetAllTranscriptsByUserAsync(USER_ID_WITH_OLD_TRANSCRIPTS))
                .ReturnsAsync(FULL_TRANSCRIPT_LIST_WITH_LASTMODIFIEDON);
            mockITranscriptDialogQueryRepository
                .Setup(p => p.GetAllTranscriptDialogByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(FULL_TRANSCRIPT_DIALOG_LIST);
            mockITranscriptQueryRepository
                .Setup(p => p.GetTranscriptByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Transcript() { Id = TRANSCRIPT_ID_WITHOUT_FILES });

            mockITranscriptPersonQueryRepository
                .Setup(p => p.GeTranscriptPersonByIdAsync(It.IsAny<int>()))
                .Returns<int>(async (transcriptPersonId) =>
                {
                    await Task.Yield();
                    if (transcriptPersonId == PERSON_ID_WITH_ADDITIONAL_INFO)
                    {
                        return TRANSCRIPT_PERSON_WITH_ADDITIONAL_INFO;
                    }
                    if (transcriptPersonId == PERSON_ID_WITHOUT_ADDITIONAL_INFO)
                    {
                        return TRANSCRIPT_PERSON_WITHOUT_ADDITIONAL_INFO;
                    }

                    return null;
                });

            MockIPersonAdditionalInfoCommandRepository
                .Setup(p => p.DeleteAdditionalInfoAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptFileQueryRepository
                .Setup(p => p.GetAllTranscriptFileByIdAsync(It.IsAny<int>()))
                .Returns<int>(async (transcriptId) =>
                {
	                await Task.Yield();
	                if (transcriptId == TRANSCRIPT_ID_WITH_FILES)
	                {
		                return FULL_TRANSCRIPT_FILE_LIST;
	                }
	                if (transcriptId == TRANSCRIPT_ID_WITHOUT_FILES)
	                {
		                return new List<TranscriptFile>();
	                }

                    return null;
                });

            MockITranscriptFileCommandRepository
                .Setup(p => p.DeleteAllFilesByTranscriptIdAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockTranscriptDialogCommandRepository
                .Setup(p => p.DeleteDialogsByTranscriptIdAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockTranscriptDialogCommandRepository
                .Setup(p => p.UpdateAllSpeakersAsync(It.IsAny<ChangeSpeakerDto>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockTranscriptDialogCommandRepository
                .Setup(p => p.UpdateSingleExaminationAsync(It.IsAny<TranscriptDialog>()))
                .ReturnsAsync(AFFECTED_ROWS);
            
            MockTranscriptDialogCommandRepository
                .Setup(p => p.UpdateMassivelyExaminationTagAsync(It.IsAny<TranscriptDialog>(), It.IsAny<TranscriptDialog>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptPersonCommandRepository
                .Setup(p => p.DeleteAllPeopleByTranscriptIdAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptPersonCommandRepository
	            .Setup(p => p.DeletePersonAsync(It.IsAny<int>()))
	            .ReturnsAsync(AFFECTED_ROWS);

            MockICourtCommandRepository
                .Setup(p => p.DeleteCourtByTranscriptIdAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptJobCommandRepository
                .Setup(p => p.DeleteAllJobsByTranscriptIdAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptCommandRepository
                .Setup(p => p.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptJobCommandRepository
	            .Setup(p => p.CreateJobAsync(It.IsAny<TranscriptJob>()))
	            .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptFileCommandRepository
	            .Setup(p => p.DeleteDuplicatedFileAsync(It.IsAny<int>()))
	            .ReturnsAsync(AFFECTED_ROWS);

            MockITranscriptFileCommandRepository
	            .Setup(p => p.SaveFileAsync(It.IsAny<TranscriptFile>()))
	            .ReturnsAsync(AFFECTED_ROWS);

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

            mockILanguageQueryRepository
                .Setup(p => p.GetAllLanguagesByTypeAsync(It.IsAny<LanguageTypeEnum>()))
                .ReturnsAsync(new List<ApiLanguage>());

            Mock<ITranscriptContext> mockITranscriptContext = new Mock<ITranscriptContext>();

            mockITranscriptContext.SetupGet(p => p.TranscriptQueryRepository)
                .Returns(mockITranscriptQueryRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptPersonQueryRepository)
                .Returns(mockITranscriptPersonQueryRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptCommandRepository)
                .Returns(MockITranscriptCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptPersonCommandRepository)
                .Returns(MockITranscriptPersonCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptFileQueryRepository)
                .Returns(MockITranscriptFileQueryRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptFileCommandRepository)
                .Returns(MockITranscriptFileCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptJobCommandRepository)
                .Returns(MockITranscriptJobCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.TranscriptDialogQueryRepository)
                .Returns(mockITranscriptDialogQueryRepository.Object);
            
            mockITranscriptContext.SetupGet(p => p.TranscriptDialogCommandRepository)
                .Returns(MockTranscriptDialogCommandRepository.Object);
            
            mockITranscriptContext.SetupGet(p => p.CourtCommandRepository)
                .Returns(MockICourtCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.PersonAdditionalInfoCommandRepository)
                .Returns(MockIPersonAdditionalInfoCommandRepository.Object);

            mockITranscriptContext.SetupGet(p => p.FileProviderFactory)
                .Returns(mockIFileProviderFactory.Object);

            mockITranscriptContext.SetupGet(p => p.UserQueryRepository)
	            .Returns(mockIUserQueryRepository.Object);
            
            mockITranscriptContext.SetupGet(p => p.LanguageQueryRepository)
	            .Returns(mockILanguageQueryRepository.Object);

            InstanceToTest = new TranscriptDomain(mockITranscriptContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task GetAllTranscriptsByUser_Success_NoTranscriptsAsync()
        {
            //ARRANGE

            //ACT
            IEnumerable<Transcript> result = await InstanceToTest.GetAllTranscriptsByUserAsync(USER_ID_WITHOUT_TRANSCRIPTS);

            //ASSERT
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTranscriptsByUser_Success_TranscriptListAsync()
        {
            //ARRANGE

            //ACT
            IEnumerable<Transcript> result = await InstanceToTest.GetAllTranscriptsByUserAsync(USER_ID_WITH_TRANSCRIPTS);

            //ASSERT
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAllTranscriptsByUser_Success_NotNullLastModifiedOnAsync()
        {
            //ARRANGE

            //ACT
            IEnumerable<Transcript> result = await InstanceToTest.GetAllTranscriptsByUserAsync(USER_ID_WITH_NEW_TRANSCRIPTS);

            //ASSERT
            Assert.NotEmpty(result);
            foreach (Transcript transcriptObject in result)
            {
                Assert.Equal(transcriptObject.CreatedOn, transcriptObject.LastModifiedOn);
            }
        }

        [Fact]
        public async Task GetAllTranscriptsByUser_Success_GetCorrectLastModifiedOnAsync()
        {
            //ARRANGE

            //ACT
            IEnumerable<Transcript> result = await InstanceToTest.GetAllTranscriptsByUserAsync(USER_ID_WITH_OLD_TRANSCRIPTS);

            //ASSERT
            Assert.NotEmpty(result);
            foreach (Transcript transcriptObject in result)
            {
                Assert.NotEqual(transcriptObject.CreatedOn, transcriptObject.LastModifiedOn);
            }
        }

        [Fact]

        public async Task DeleteTranscriptPersonAsync_SuccessWith_AdditionalInfoTestAsync()
        {
            int result = await InstanceToTest.DeleteTranscriptPersonAsync(PERSON_ID_WITH_ADDITIONAL_INFO);

            Assert.True(result >= AFFECTED_ROWS);
            MockIPersonAdditionalInfoCommandRepository.Verify(m => m.DeleteAdditionalInfoAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task DeleteTranscriptPersonAsync_SuccessWithout_AdditionalInfoTestAsync()
        {
            int result = await InstanceToTest.DeleteTranscriptPersonAsync(PERSON_ID_WITHOUT_ADDITIONAL_INFO);

            Assert.True(result >= AFFECTED_ROWS);
            MockIPersonAdditionalInfoCommandRepository.Verify(m => m.DeleteAdditionalInfoAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTranscriptAsync_SuccessTestAsync()
        {
            int result = await InstanceToTest.DeleteTranscriptAsync(1);
            
            Assert.True(result > AFFECTED_ROWS);
            MockITranscriptFileQueryRepository.Verify(m => m.GetAllTranscriptFileByIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockITranscriptFileCommandRepository.Verify(m => m.DeleteAllFilesByTranscriptIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockTranscriptDialogCommandRepository.Verify(m => m.DeleteDialogsByTranscriptIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockITranscriptPersonCommandRepository.Verify(m => m.DeleteAllPeopleByTranscriptIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockICourtCommandRepository.Verify(m => m.DeleteCourtByTranscriptIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockITranscriptJobCommandRepository.Verify(m => m.DeleteAllJobsByTranscriptIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            MockITranscriptCommandRepository.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task SaveTranscriptMediaInformationAsync_WithUserApiUrl_SuccessTestAsync()
        {
	        var file = new Mock<IFormFile>();
	        TranscriptFile transcriptFile = new TranscriptFile()
	        {
		        File = file.Object,
		        Name = "fake file name",
		        Path = "fake path",
		        TranscriptId = TRANSCRIPT_ID_WITHOUT_FILES,
		        Size = 200,
                CreatedBy = USER_ID_WITH_API_URL
	        };

	        var result = await InstanceToTest.SaveTranscriptMediaInformationAsync(transcriptFile);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task SaveTranscriptMediaInformationAsync_WithoutUserApiUrl_SuccessTestAsync()
        {
	        var file = new Mock<IFormFile>();
	        TranscriptFile transcriptFile = new TranscriptFile()
	        {
		        File = file.Object,
		        Name = "fake file name",
		        Path = "fake path",
		        TranscriptId = TRANSCRIPT_ID_WITHOUT_FILES,
		        Size = 200,
		        CreatedBy = USER_ID_WITHOUT_API_URL
	        };

	        var result = await InstanceToTest.SaveTranscriptMediaInformationAsync(transcriptFile);

	        Assert.NotNull(result);
        }

        [Fact]
        public async Task SaveTranscriptMediaInformationAsync_WithFilesExists_SuccessTestAsync()
        {
	        var file = new Mock<IFormFile>();
	        TranscriptFile transcriptFile = new TranscriptFile()
	        {
		        File = file.Object,
		        Name = "fake file name",
		        Path = "fake path",
		        TranscriptId = TRANSCRIPT_ID_WITH_FILES,
		        Size = 200,
		        CreatedBy = USER_ID_WITHOUT_API_URL
	        };

	        var result = await InstanceToTest.SaveTranscriptMediaInformationAsync(transcriptFile);

	        Assert.Equal(result.Id, transcriptFile.TranscriptId);
        }

        [Fact]
        public async Task UpdateAllSpeakersAsync_SuccessTestAsync()
		{
            var result = await InstanceToTest.UpdateAllSpeakersAsync(new ChangeSpeakerDto());

            Assert.Equal(AFFECTED_ROWS, result);
        }

        [Fact]

        public async Task SaveExaminationTagsAsync_SuccessAsync()
		{
            IEnumerable<SaveSingleExaminationTagDto> dtoList = new List<SaveSingleExaminationTagDto>()
            {
                new SaveSingleExaminationTagDto()
            };

            var result = await InstanceToTest.SaveExaminationTagsAsync(dtoList);
            Assert.True(result >=  AFFECTED_ROWS);
        }

        [Fact]

        public async Task UpdateSingleExaminationTagAsync_SuccessAsync()
		{
            var result = await InstanceToTest.UpdateSingleExaminationTagAsync(new SaveSingleExaminationTagDto());
            Assert.True(result >=  AFFECTED_ROWS);
        }

        [Fact]
        public async Task UpdateMassivelyExaminationTagAsync_SuccessAsync()
		{
            var result = await InstanceToTest.UpdateMassivelyExaminationTagAsync(new SaveMassiveExaminationTagDto());
            Assert.True(result >=  AFFECTED_ROWS);
        }

        [Fact]
        public async Task GetAllTranscriptionLanguagesAsync_SuccessAsync()
		{
            var result = await InstanceToTest.GetAllTranscriptionLanguagesAsync();
            Assert.NotNull(result);
        }

        #endregion
    }
}
