using Authenticity.CourtSide.Core.DataProviders;
using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains.Context;
using Authenticity.CourtSide.Core.Domains.Implementation;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Authenticity.CourtSide.Test.Unit.Domains
{
    [Trait("UnitTest", "ExportDomain")]
    public class ExportDomainTests
    {
        #region [Setup]

        private static readonly int USER_ID_WITHOUT_TRANSCRIPTS = 1;
        private static readonly int USER_ID_WITH_TRANSCRIPTS = 2;
        private static readonly int USER_ID_WITH_NEW_TRANSCRIPTS = 3;
        private static readonly int USER_ID_WITH_OLD_TRANSCRIPTS = 4;
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

        private static readonly FileExportFormat DEFAULT_FILE_EXPORT_FORMAT = new FileExportFormat()
        {
            FileName = "Fake export format file name",
            FormatName = "Fake format name",
            Path = "Fake path"
        };

        private ExportDomain InstanceToTest { get; }

        public ExportDomainTests()
        {
            Mock<ITranscriptQueryRepository> mockITranscriptQueryRepository = new Mock<ITranscriptQueryRepository>();

            Mock<ITranscriptPersonQueryRepository> mockITranscriptPersonQueryRepository = new Mock<ITranscriptPersonQueryRepository>();

            Mock<ITranscriptDialogQueryRepository> mockITranscriptDialogQueryRepository = new Mock<ITranscriptDialogQueryRepository>();
            Mock<IFileExportFormatQueryRepository> mockFileExportFormatQueryRepository = new Mock<IFileExportFormatQueryRepository>();

            Mock<IFileExporter> mockFileExporter = new Mock<IFileExporter>();
            Mock<ICourtQueryRepository> mockICourtQueryRepository = new Mock<ICourtQueryRepository>();

            Mock<IFileProvider> mockIFileProvider = new Mock<IFileProvider>();
            
            Mock<IFileProviderFactory> mockIFileProviderFactory = new Mock<IFileProviderFactory>();

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
                .ReturnsAsync(new Transcript());
            mockIFileProvider
                .Setup(p => p.GetFileToLocalTempDirectoryAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(TRANSCRIPT_EXPORT_FILE_TEMPLATE_PATH);
            mockFileExporter
                .Setup(p => p.CreateExportDocument(It.IsAny<string>(), It.IsAny<Transcript>(),
                    It.IsAny<IEnumerable<TranscriptPerson>>(), It.IsAny<IEnumerable<TranscriptDialog>>(), It.IsAny<int>()))
                .Returns(TRANSCRIPT_EXPORT_FILE_TEMPLATE_PATH);
            mockIFileProviderFactory
                .Setup(p => p.CreateAsync())
                .ReturnsAsync(mockIFileProvider.Object);

            Mock<IExportContext> mockIExportContext = new Mock<IExportContext>();

            mockIExportContext.SetupGet(p => p.TranscriptQueryRepository)
                .Returns(mockITranscriptQueryRepository.Object);
            
            mockIExportContext.SetupGet(p => p.TranscriptPersonQueryRepository)
                .Returns(mockITranscriptPersonQueryRepository.Object);

            mockIExportContext.SetupGet(p => p.TranscriptDialogQueryRepository)
                .Returns(mockITranscriptDialogQueryRepository.Object);

            mockIExportContext.SetupGet(p => p.FileExportFormatQueryRepository)
                .Returns(mockFileExportFormatQueryRepository.Object);

            mockIExportContext.SetupGet(p => p.FileExporter)
                .Returns(mockFileExporter.Object);
            
            mockIExportContext.SetupGet(p => p.FileProviderFactory)
                .Returns(mockIFileProviderFactory.Object);

            InstanceToTest = new ExportDomain(mockIExportContext.Object);
        }

        #endregion

        #region [Tests]

        [Fact]
        public async Task GetTranscriptPreviewLinesAsync_Success()
        {
            //ARRANGE

            //ACT
            string[] result = await InstanceToTest.GetTranscriptPreviewLinesAsync(1);

            //ASSERT
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task ExportTranscriptAsync_Success_PDFAsync()
        {
            //ARRANGE
            ExportTranscriptDto exportTranscript = new ExportTranscriptDto()
            {
                TranscriptId = 1,
                Extension = TranscriptExportFormatEnum.Pdf
            };

            //ACT
            FileStream result = await InstanceToTest.ExportTranscriptToWordFormatAsync(exportTranscript);

            //ASSERT
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ExportTranscriptAsync_Success_TXTAsync()
        {
            //ARRANGE
            ExportTranscriptDto exportTranscript = new ExportTranscriptDto()
            {
                TranscriptId = 1,
                Extension = TranscriptExportFormatEnum.Pdf
            };

            //ACT
            FileStream result = await InstanceToTest.ExportTranscriptToWordFormatAsync(exportTranscript);

            //ASSERT
            Assert.NotNull(result);
        }

        #endregion
    }
}
