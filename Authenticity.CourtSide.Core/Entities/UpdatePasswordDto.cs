using Authenticity.CourtSide.Core.Enums;

namespace Authenticity.CourtSide.Core.Entities
{
    public class UpdatePasswordDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
