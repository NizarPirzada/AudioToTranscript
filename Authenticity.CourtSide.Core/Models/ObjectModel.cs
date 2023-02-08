using System.Collections.Generic;

namespace Authenticity.CourtSide.Core.Models
{
    public class ObjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PermissionModel> Permissions { get; set; } = new HashSet<PermissionModel>();
    }
}
