using Authenticity.CourtSide.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;

namespace Authenticity.CourtSide.Core.Models
{
    public class TranscriptFile : Timestamp
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public string Name{ get; set; }
        public string Path { get; set; }
        public int TranscriptId { get; set; }
        public decimal Size { get; set; }
    }
}
