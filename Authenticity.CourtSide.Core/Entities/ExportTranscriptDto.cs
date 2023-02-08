using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Entities
{
    public class ExportTranscriptDto
    {
        [Required(ErrorMessage = "Transcript Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please check the Transcript Id")]
        [JsonProperty("transcriptId")]
        public int TranscriptId { get; set; }

        [Required(ErrorMessage = "Extension is required")]
        [JsonProperty("extension")]
        public TranscriptExportFormatEnum Extension { get; set; }

        public int FormatId { get; set; }

        public int Offset { get; set; }
    }
}
