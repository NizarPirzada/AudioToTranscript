using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Authenticity.CourtSide.Core.Helpers;
using Newtonsoft.Json;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
    public class AuthenticityTranscriptConversationResponse
    {
        [JsonProperty("stime")]
        public decimal StartTime { get; set; }
        [JsonProperty("duration")]
        public decimal Duration { get; set; }
        [JsonProperty("content")]
        public string Transcription { get; set; }
        [JsonProperty("speaker")]
        public string Speaker { get; set; }
    }
}
