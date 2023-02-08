using System.Collections.Generic;

namespace Authenticity.CourtSide.Core.Models
{
    public class RoleModel: Timestamp
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ObjectPermissionModel> ObjectPermissions { get; set; } = new HashSet<ObjectPermissionModel>();
    }
}
