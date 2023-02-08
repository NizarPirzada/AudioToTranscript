using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class LoginDto
    {
        internal const string EMAIL_REQUIRED_MESSAGE = "Email is required";
        internal const string EMAIL_REQUIRED_LENGTH_MESSAGE = "Email field must not exceed 100 characters";
        internal const string PASSWORD_REQUIRED_MESSAGE = "Password is required";
        internal const string PASSWORD_REQUIRED_LENGTH_MESSAGE = "Password field must not exceed 450 characters";

        [Required(ErrorMessage = EMAIL_REQUIRED_MESSAGE)]
        [StringLength(100, ErrorMessage = EMAIL_REQUIRED_LENGTH_MESSAGE)]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = PASSWORD_REQUIRED_MESSAGE)]
        [StringLength(450, ErrorMessage = PASSWORD_REQUIRED_LENGTH_MESSAGE)]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
