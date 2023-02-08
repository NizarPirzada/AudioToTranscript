using Newtonsoft.Json;

namespace Authenticity.CourtSide.Core.Entities
{
    public class BaseResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
