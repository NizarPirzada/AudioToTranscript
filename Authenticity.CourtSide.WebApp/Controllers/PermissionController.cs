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
    public class PermissionController : ApiControllerBase
    {
        private IPermissionDomain PermissionDomain { get; }
        private ILogger<PermissionController> Logger { get; }

        public PermissionController(IPermissionDomain permissionDomain, ILogger<PermissionController> logger)
        {
            PermissionDomain = permissionDomain;
            Logger = logger;
        }

        [HttpGet]
        [Route("GetAllObjects")]
        public async Task<IActionResult> GetAllObjectPermissionByUserAsync()
        {
            try
            {
                var userRequest = TokenGenerator.GetUserByRequest(Request);
                var objectPermissions = await PermissionDomain.GetObjectPermissionByUserAsync(userRequest);

                BaseResponse<IEnumerable<ObjectModel>> response = new BaseResponse<IEnumerable<ObjectModel>>()
                {
                    Data = objectPermissions,
                    Success = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "GetAllObjctPermissionByUserAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }


        [HttpGet]
        [Route("CheckUserPermission/{permission}")]
        public async Task<IActionResult> CheckUserPermissionAsync(string permission)
        {
            try
            {
                var userRequest = TokenGenerator.GetUserByRequest(Request);

                var hasUserPermissions = await PermissionDomain.CheckUserPermissionAsync(userRequest, permission);

                BaseResponse<bool> response = new BaseResponse<bool>()
                {
                    Success = true,
                    Data = hasUserPermissions
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CheckUserPermissionAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }

        [HttpGet]
        [Route("CheckTranscriptPermission/{transcriptId}")]
        public async Task<IActionResult> CheckTranscriptPermissionAsync(int transcriptId)
        {
            try
            {
                UserModel userRequest = TokenGenerator.GetUserByRequest(Request);

                var hasUserPermissions = await PermissionDomain.CheckTranscriptPermissionAsync(userRequest.Id, transcriptId);

                BaseResponse<bool> response = new BaseResponse<bool>()
                {
                    Success = hasUserPermissions,
                    Data = hasUserPermissions
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "CheckUserPermissionAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }
    }
}
