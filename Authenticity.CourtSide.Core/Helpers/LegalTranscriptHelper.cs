using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Helpers
{
    public static class LegalTranscriptHelper
    {
        public static List<string> GetTranscriptionWithSpeakers(IEnumerable<TranscriptPerson> speakers, IEnumerable<TranscriptDialog> dialogs)
        {
            List<string> transcriptWithSpeakers = new List<string>();

            foreach (var dialog in dialogs)
            {
                string textFormatted = $"{ GetSpeakerName(dialog, speakers)}: { dialog.Transcription}";
                transcriptWithSpeakers.Add(textFormatted);
            }

            return transcriptWithSpeakers;
        }

        public static IEnumerable<TranscriptDialog> SetTranscriptionWithSpeakerName(IEnumerable<TranscriptPerson> speakers, IEnumerable<TranscriptDialog> dialogs)
        {
            foreach (var dialog in dialogs)
            {
                dialog.Transcription = $"{ GetSpeakerName(dialog, speakers)}: { dialog.Transcription}";
            }

            return dialogs;
        }

        public static string GetSpeakerName(TranscriptDialog dialog, IEnumerable<TranscriptPerson> speakers)
        {
            string speakerName = dialog.OriginalSpeakerName?.Length == 1 ? $"Speaker {dialog.OriginalSpeakerName}" : dialog.OriginalSpeakerName;
            if (dialog.PersonId != 0)
            {
                var personSelected = speakers.Where(s => s.Id == dialog.PersonId).FirstOrDefault();

                if (personSelected != null)
                {
                    speakerName = $"{personSelected.FirstName} { personSelected.LastName}";
                }
            }
            return speakerName.ToUpper();
        }
    }
}
