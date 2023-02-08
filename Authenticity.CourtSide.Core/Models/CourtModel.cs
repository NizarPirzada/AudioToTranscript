using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticity.CourtSide.Core.Models
{
    public class CourtModel : Timestamp
    {
        public int CourtId { get; set; }
        public int TranscriptId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
