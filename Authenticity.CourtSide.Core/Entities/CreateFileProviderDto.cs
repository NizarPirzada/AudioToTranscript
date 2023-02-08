using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class CreateFileProviderDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name field must not exceed 100 characters")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Parameters is required")]
        [JsonProperty("parameters")]
        public string Parameters { get; set; }

        [Required(ErrorMessage = "IsCurrentProvider is required")]
        [JsonProperty("isCurrentProvider")]
        public bool IsCurrentProvider { get; set; }

        public int CreatedBy { get; set; }
    }
}
