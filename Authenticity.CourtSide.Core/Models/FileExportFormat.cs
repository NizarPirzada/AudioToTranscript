using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Authenticity.CourtSide.Core.Models
{
    public class FileExportFormat : Timestamp
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public string FormatName { get; set; }

        public string Type { get; set; }
    }
}
