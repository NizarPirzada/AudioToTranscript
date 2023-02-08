using Authenticity.CourtSide.Core.Models;
using Newtonsoft.Json;
using System;

namespace Authenticity.CourtSide.Core.Entities
{
    public class LoginResponseDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTime? Expires { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }
    }
}
