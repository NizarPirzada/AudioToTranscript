using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Authenticity.CourtSide.Core.Helpers;
using Newtonsoft.Json;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
    public class AuthenticityTranscriptEngineResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("task_id")]
        public string TaskId { get; set; }
        [JsonProperty("task_url")]
        public string TaskUrl { get; set; }
    }
}
