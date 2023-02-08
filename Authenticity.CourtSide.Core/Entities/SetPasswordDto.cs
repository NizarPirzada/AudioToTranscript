using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class SetPasswordDto
    {
        [Required(ErrorMessage = "Email activation Id is required")]
        [JsonProperty("emailActivationId")]
        public string EmailActivationId { get; set; }

        [Required(ErrorMessage = Constants.PASSWORD_REQUIRED_MESSAGE)]
        [RegularExpression(Constants.PASSWORD_POLICY_REGEX, ErrorMessage = Constants.PASSWORD_POLICY_MESSAGE)]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = Constants.PASSWORD_DO_NOT_MATCH_MESSAGE)]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
