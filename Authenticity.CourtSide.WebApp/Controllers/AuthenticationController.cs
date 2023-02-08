using Authenticity.CourtSide.Core.Domains;
using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ApiControllerBase
    {
        private IAuthenticationDomain AuthenticationService { get; }
        private ILogger<AuthenticationController> Logger { get; }
        private IConfiguration Configuration { get; }

        public AuthenticationController(IAuthenticationDomain authenticationService, ILogger<AuthenticationController> logger, IConfiguration configuration)
        {
            AuthenticationService = authenticationService;
            Logger = logger;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(v => new { key = v.Key, errors = v.Value.Errors.Select(y => y.ErrorMessage) });
                    return BadRequest(new { success = false, errors, message = "Invalid operation, please check the credentials." });
                }

                int expirationTokenTime = Configuration.GetValue<int>("ExpirationTokenTimeInMinutes");

                LoginResponseDto loginUserResponse = await AuthenticationService.LoginUserAsync(loginDto, expirationTokenTime);
                loginUserResponse.User.Password = string.Empty;

                BaseResponse<LoginResponseDto> response = new BaseResponse<LoginResponseDto>()
                {
                    Data = loginUserResponse,
                    Success = true
                };

                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex.Message);
                return BadRequest(invalidOperationException);
            }
            catch(InvalidDataException ex)
            {
                var invalidDataException = ErrorResponseHelper.GetErrorResponse(ex.Message);
                return BadRequest(invalidDataException);
            }
            catch (Exception ex)
            {
                var invalidOperationException = ErrorResponseHelper.GetErrorResponse(ex, "LoginAsync", Logger);
                return BadRequest(invalidOperationException);
            }
        }
    }
}
