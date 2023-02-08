using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name field must not exceed 100 characters")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name field must not exceed 100 characters")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email field must not exceed 100 characters")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "StatusId must be selected")]
        [JsonProperty("status")]
        public UserStatusEnum Status { get; set; } = UserStatusEnum.Pending;

        [Required(ErrorMessage = "Role must be selected")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the role information sent")]
        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        public string EmailActivationId { get; set; }

        public int CreatedBy { get; set; }

        public string ApiUrl { get; set; }
        
        public string ApiGuid { get; set; }

        public int? TranscriptionEngineId { get; set; }
    }
}
