using Authenticity.CourtSide.Core.Enums;

namespace Authenticity.CourtSide.Core.Entities
{
    public class UpdateUserStatusDto
    {
        public int Id { get; set; }
        public UserStatusEnum Status { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
