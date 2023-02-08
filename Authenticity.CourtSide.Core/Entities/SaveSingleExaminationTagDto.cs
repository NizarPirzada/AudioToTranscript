using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class SaveSingleExaminationTagDto
    {
        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Transcript Id")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        [Required(ErrorMessage = "Dialog Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Dialog Id")]
        [JsonProperty("dialogId")]
        public int DialogId { get; set; }

        [Required(ErrorMessage = "Examination Tag is required")]
        [JsonProperty("examinationTag")]
        public TranscriptExaminationTagEnum ExaminationTag { get; set; }
    }
}
