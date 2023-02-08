using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class ChangePasswordDto
	{
        [Required(ErrorMessage = "Current Password is required")]
        [JsonProperty("currentPassword")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = Constants.PASSWORD_REQUIRED_MESSAGE)]
        [RegularExpression(Constants.PASSWORD_POLICY_REGEX, ErrorMessage = Constants.PASSWORD_POLICY_MESSAGE)]
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = Constants.PASSWORD_DO_NOT_MATCH_MESSAGE)]
        [JsonProperty("confirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public int UserId { get; set; }
    }
}
