namespace Authenticity.CourtSide.Core.Models
{
    public class ExportTranscriptModel
    {
        public Transcript Transcript { get; set; }
        public string[] FormatedDialog { get; set; }
        public decimal TotalPages { get; set; }
        public string[] PlaintiffNames { get; set; }
        public string[] DeponentNames { get; set; }
        public string[] TextCourtInfoLines { get; set; }
        public string[] TextAppearancesInfoLines { get; set; }
    }
}
