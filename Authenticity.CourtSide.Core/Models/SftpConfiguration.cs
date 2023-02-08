namespace Authenticity.CourtSide.Core.Models
{
    public class SftpConfiguration
    {
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string RootFolder { get; set; }
    }
}
