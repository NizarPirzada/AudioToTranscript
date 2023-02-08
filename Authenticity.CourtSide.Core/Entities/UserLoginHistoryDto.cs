using System;

namespace Authenticity.CourtSide.Core.Entities
{
    public class UserLoginHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
