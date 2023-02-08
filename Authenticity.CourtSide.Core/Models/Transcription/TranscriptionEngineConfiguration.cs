using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticity.CourtSide.Core.Models.Transcription
{
    public class TranscriptionEngineConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EndPointURL { get; set; }
    }
}
