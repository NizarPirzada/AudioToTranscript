namespace Authenticity.CourtSide.Core.Models
{
    public class FtpConfiguration
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSSL { get; set; }
    }
}
