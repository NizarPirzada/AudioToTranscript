using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class EditUserDto 
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the id sent")]
        [JsonProperty("id")]
        public int Id { get; set; }

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
        public UserStatusEnum Status { get; set; } = UserStatusEnum.Active;

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [JsonProperty("apiGuid")]
        public string ApiGuid { get; set; }

        [JsonProperty("transcriptionEngineId")]
        public int TranscriptionEngineId { get; set; }

        public int LastModifiedBy { get; set; }
        
        public string EmailActivationId { get; set; }
    }
}
