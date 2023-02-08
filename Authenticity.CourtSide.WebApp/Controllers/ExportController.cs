using Authenticity.CourtSide.Core.DataProviders.FileProvider;
using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExportController : ApiControllerBase
    {
        public ExportController(IExportDomain exportDomain, ILogger<ExportController> logger)
        {
            ExportDomain = exportDomain;
            Logger = logger;
        }
        private IExportDomain ExportDomain { get; }
        private ILogger<ExportController> Logger { get; }


        [HttpGet]
        [Route("GetTranscriptPreviewLines/{transcriptId}")]
        public async Task<IActionResult> GetTranscriptPreviewLinesAsync(int transcriptId)
        {
            try
            {
                string[] result = await ExportDomain.GetTranscriptPreviewLinesAsync(transcriptId);

                BaseResponse<string[]> response = new BaseResponse<string[]>()
                {
                    Data = result,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetTranscriptPreviewLinesAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }

        [HttpPost]
        [Route("ExportTranscript")]
        public async Task<IActionResult> ExportTranscriptAsync([FromBody] ExportTranscriptDto exportTranscriptDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
                    return BadRequest(new { success = false, errors, message = "Invalid transcript data." });
                }

                FileStreamResult file = null;

                switch (exportTranscriptDto.Extension)
                {
                    case TranscriptExportFormatEnum.WordFormat:
                        FileStream streamFile = await ExportDomain.ExportTranscriptToWordFormatAsync(exportTranscriptDto);
                        file = new FileStreamResult(streamFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                        break;
                    default:
                        throw new ArgumentException("Transcript export format is not valid");
                }

                return file;
            }
            catch (ArgumentException ex)
            {
                var argumentException = ErrorResponseHelper.GetErrorResponse(ex.Message);
                return BadRequest(argumentException);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "ExportTranscriptAsync", Logger);
                return BadRequest(invalidOperationException);
            }

        }

        [HttpPost]
        [Route("TranslateExportTranscript")]
        public async Task<IActionResult> TranslateAndExportTranscriptAsync([FromBody] ExportTranslateTranscriptDto exportTranscriptDto)
        {
            try
            {
                exportTranscriptDto.UserId = UserId;
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
                    return BadRequest(new { success = false, errors, message = "Invalid transcript data." });
                }

                FileStreamResult file = null;

                switch (exportTranscriptDto.Extension)
                {
                case TranscriptExportFormatEnum.WordFormat:
                        FileStream streamFile = await ExportDomain.TranslateAndExportTranscriptToWordFormatAsync(exportTranscriptDto);
                        file = new FileStreamResult(streamFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                        break;
                    default:
                        throw new ArgumentException("Transcript export format is not valid");
                }

                return file;
            }
            catch (ArgumentException ex)
            {
                var argumentException = ErrorResponseHelper.GetErrorResponse(ex.Message);
                return BadRequest(argumentException);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "TranslateExportTranscript", Logger);
                return BadRequest(invalidOperationException);
            }

        }

        [HttpGet]
        [Route("GetAllTranslationLanguages")]
        public async Task<IActionResult> GetAllTranslationLanguagesAsync()
        {
            try
            {
                var result = await ExportDomain.GetAllTranslationLanguagesAsync();

                BaseResponse<IEnumerable<ApiLanguage>> response = new BaseResponse<IEnumerable<ApiLanguage>>()
                {
                    Data = result,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllTranslationLanguagesAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }
    }
}
