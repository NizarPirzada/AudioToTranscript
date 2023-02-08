namespace Authenticity.CourtSide.Core.Models
{
    public class FileProviderModel : Timestamp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public bool IsCurrentProvider { get; set; }
    }
}
