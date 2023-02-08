namespace Authenticity.CourtSide.Core.Models
{
    public class ObjectPermissionModel
    {
        public int ObjectPermissionId { get; set; }

        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public int ObjectId { get; set; }

        public string ObjectName { get; set; }
    }
}
