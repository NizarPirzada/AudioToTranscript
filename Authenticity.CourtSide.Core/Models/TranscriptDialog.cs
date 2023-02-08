using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Models
{
	public class TranscriptDialog : Timestamp
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Id sent")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Transcript Id sent")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        public int OriginalSpeakerId { get; set; }
        public string OriginalSpeakerName { get; set; }

        [Required(ErrorMessage = "Person Id is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please check the Person Id sent")]
        [JsonProperty("personId")]
        public int PersonId { get; set; }

        public decimal StartTime { get; set; }
        
        public decimal Duration { get; set; }

        [Required(ErrorMessage = "Transcription is required")]
        [JsonProperty("transcription")]
        public string Transcription { get; set; }

        [Required(ErrorMessage = "Examination is required")]
        [JsonProperty("examination")]
        public TranscriptExaminationEnum ExaminationType { get; set; }

        [Required(ErrorMessage = "Examination tag is required")]
        [JsonProperty("examinationTag")]
        public TranscriptExaminationTagEnum ExaminationTag { get; set; }

        public decimal EndTime
        {
            get
            {
                return StartTime + Duration;
            }
        }
    }
}
