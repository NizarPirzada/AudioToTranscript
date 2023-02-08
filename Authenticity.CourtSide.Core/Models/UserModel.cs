using Authenticity.CourtSide.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Authenticity.CourtSide.Core.Models
{
    public class UserModel : Timestamp
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        [JsonIgnore]
        public string Password { get; set; }
        public UserStatusEnum Status { get; set; }
        public ICollection<RoleModel> Roles { get; set; } = new HashSet<RoleModel>();
        public string EmailActivationId { get; set; }
        public DateTime? LastLogin { get; set; }
        public string ApiUrl { get; set; }
        public string ApiGuid { get; set; }
		public int TranscriptionEngineId { get; set; }
	}
}
