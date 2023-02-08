using System;

namespace Authenticity.CourtSide.Core.Models
{
    public class Timestamp
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
