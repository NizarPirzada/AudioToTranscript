using System;
using System.Collections.Generic;
using System.Text;

namespace Authenticity.CourtSide.Core.Entities
{
    public class EditUserRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int LastModifiedBy { get; set; }
    }
}
