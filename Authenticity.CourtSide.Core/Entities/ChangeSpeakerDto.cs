using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class ChangeSpeakerDto
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Id sent")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the transcript Id sent")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        [Required(ErrorMessage = "Person Id is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please check the person Id sent")]
        [JsonProperty("personId")]
        public int PersonId { get; set; }
    }
}
