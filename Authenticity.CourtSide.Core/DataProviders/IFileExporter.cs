using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Models.Transcription;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders
{
    public interface IFileExporter
    {
        string CreateExportDocument(string pathFile, Transcript transcript, IEnumerable<TranscriptPerson> persons, IEnumerable<TranscriptDialog> formatedDialog, int offset);
    }
}
