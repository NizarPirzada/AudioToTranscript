using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class UpdateFileProviderDto
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the id sent")]
        [JsonProperty("id")]
        public int Id { get; set; }

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

        public int LastModifiedBy { get; set; }
    }
}
