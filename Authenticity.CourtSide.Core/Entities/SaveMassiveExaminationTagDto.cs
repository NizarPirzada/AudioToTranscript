using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class SaveMassiveExaminationTagDto
    {
        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Transcript Id")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        [Required(ErrorMessage = "Examination Tag is required")]
        [JsonProperty("examinationtag")]
        public TranscriptExaminationTagEnum ExaminationTag { get; set; }

        [Required(ErrorMessage = "Original Speaker Idis required")]
        [JsonProperty("originalSpeakerId")]
        public int OriginalSpeakerId { get; set; }

        [Required(ErrorMessage = "Start dialog Id is required")]
        [JsonProperty("startExaminationId")]
        public int StartExaminationId { get; set; }

        [Required(ErrorMessage = "End dialog Id is required")]
        [JsonProperty("closeExaminationId")]
        public int CloseExaminationId { get; set; }
    }
}
