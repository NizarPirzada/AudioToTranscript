using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class CheckUserApiDto
    {
        [Required(ErrorMessage = "User API URL is required")]
        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [Required(ErrorMessage = "User API GUID is required")]
        [JsonProperty("apiGuid")]
        public string ApiGuid { get; set; }
    }
}
