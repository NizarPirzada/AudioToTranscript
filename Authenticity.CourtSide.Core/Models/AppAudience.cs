namespace Authenticity.CourtSide.Core.Models
{
    public class AppAudience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public int ExpireTimeInMinutes { get; set; }
    }
}
