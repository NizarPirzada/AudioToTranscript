using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class PasswordRecoveryDto
	{
        [Required(ErrorMessage = "Email is required")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.PASSWORD_REQUIRED_MESSAGE)]
        [JsonProperty("temporalPassword")]
        public string TemporalPassword { get; set; }

        [Required(ErrorMessage = Constants.PASSWORD_REQUIRED_MESSAGE)]
        [RegularExpression(Constants.PASSWORD_POLICY_REGEX, ErrorMessage = Constants.PASSWORD_POLICY_MESSAGE)]
        [JsonProperty("password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm password is required")]
        [Compare(nameof(Password), ErrorMessage = Constants.PASSWORD_DO_NOT_MATCH_MESSAGE)]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
