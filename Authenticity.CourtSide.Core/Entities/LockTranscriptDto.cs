using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class LockTranscriptDto
	{
        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Transcript Id sent")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        [Required(ErrorMessage = "Locked value is required")]
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        public int LastModifiedBy { get; set; }
    }
}
