using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.WebApp.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class FileController : ApiControllerBase
    {
        public FileController(IFileDomain fileDomain, ILogger<FileController> logger)
        {
            FileDomain = fileDomain;
            Logger = logger;
        }
        private IFileDomain FileDomain { get; }
        private ILogger<FileController> Logger { get; }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4294967295)]
        [Route("UploadAudioFile")]
        [Authorize]
        public async Task<IActionResult> UploadAudioFileAsync([FromForm] TranscriptFile transcript)
        {
            string filePath = string.Empty;
            string ftpFileName = string.Empty;
            try
            {
                if (transcript.File.Length > 0)
                {
                    filePath = await FileDomain.SaveFileTempLocationAsync(transcript);
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    ftpFileName = await FileDomain.SaveFileToFTPAsync(transcript.Id, transcript.Size, filePath);
                }

                BaseResponse<string> response = new BaseResponse<string>()
                {
                    Data = ftpFileName,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error raised UploadAudioFileAsync {ftpFileName}");
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UploadAudioFileAsync", Logger);
                return BadRequest(invalidOperationException);
            }
            finally
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    await FileDomain.DeleteFileTempLocationAsync(filePath);
                }
            }
        }

        [HttpGet]
        [Route("GetAudioFile")]
        public async Task<IActionResult> GetAudioFileAsync([FromQuery] int transcriptionId, [FromQuery] string tid)
        {
            try
            {
                if (int.TryParse(TokenGenerator.GetClaimValue(tid, CourtsideClaimTypes.Id), out int userRequestId))
                {
                    FileStream fileStream = await FileDomain.GetAudioFileAsync(transcriptionId);
                    FileStreamResult res = File(fileStream, "audio/mp3", true);

                    return res;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (FileNotFoundException ex)
            {
                var response = ErrorResponseHelper.GetErrorResponse(ex.Message);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAudioFileAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }
    }
}
