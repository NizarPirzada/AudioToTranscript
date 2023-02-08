using System;

namespace Authenticity.CourtSide.Core.Models
{
    public class MemoryCacheConfiguration
    {
        public int ExpirationTimeInMinutes { get; set; }
        public Guid RolesKey { get; set; }
    }
}
