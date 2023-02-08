using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ApiControllerBase
    {
        public SettingsController(ISettingsDomain settingsDomain, ILogger<SettingsController> logger)
        {
            SettingsDomain = settingsDomain;
            Logger = logger;
        }
        private ISettingsDomain SettingsDomain { get; }
        private ILogger<SettingsController> Logger { get; }


        [HttpGet]
        [Route("GetAllFileProviders")]
        public async Task<IActionResult> GetAllFileProvidersAsync()
        {
            try
            {
                IEnumerable<FileProviderModel> fileProviders = await SettingsDomain.GetAllFileProvidersAsync();

                BaseResponse<IEnumerable<FileProviderModel>> response = new BaseResponse<IEnumerable<FileProviderModel>>()
                {
                    Data = fileProviders,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllFileProvidersAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }

        [HttpPost]
        [Route("CreateFileProvider")]
        public async Task<IActionResult> CreateFileProviderAsync(CreateFileProviderDto createFileProviderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
                    return BadRequest(new { success = false, errors, message = "Invalid file provider data." });
                }

                var userRequest = TokenGenerator.GetUserByRequest(Request);
                createFileProviderDto.CreatedBy = userRequest.Id;

                FileProviderModel fileProviderCreated = await SettingsDomain.CreateFileProviderAsync(createFileProviderDto);

                BaseResponse<FileProviderModel> response = new BaseResponse<FileProviderModel>()
                {
                    Data = fileProviderCreated,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CreateFileProviderAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }

        [HttpPut]
        [Route("UpdateFileProvider")]
        public async Task<IActionResult> UpdateFileProviderAsync(UpdateFileProviderDto updateFileProviderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
                    return BadRequest(new { success = false, errors, message = "Invalid file provider data." });
                }

                var userRequest = TokenGenerator.GetUserByRequest(Request);
                updateFileProviderDto.LastModifiedBy = userRequest.Id;

                FileProviderModel fileProviderCreated = await SettingsDomain.UpdateFileProviderAsync(updateFileProviderDto);

                BaseResponse<FileProviderModel> response = new BaseResponse<FileProviderModel>()
                {
                    Data = fileProviderCreated,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "UpdateFileProviderAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }
    }
}
