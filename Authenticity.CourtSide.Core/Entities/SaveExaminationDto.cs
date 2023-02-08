using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
	public class SaveExaminationDto
    {
        [Required(ErrorMessage = "Examination is required")]
        [JsonProperty("examinationType")]
        public TranscriptExaminationEnum ExaminationType { get; set; }

        [Required(ErrorMessage = "Dialog Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Dialog Id")]
        [JsonProperty("dialogId")]
        public int DialogId { get; set; }

        [Required(ErrorMessage = "Examination Tag is required")]
        [JsonProperty("examinationTag")]
        public TranscriptExaminationTagEnum ExaminationTag { get; set; }

        public int LastModifiedBy { get; set; }
    }
}
