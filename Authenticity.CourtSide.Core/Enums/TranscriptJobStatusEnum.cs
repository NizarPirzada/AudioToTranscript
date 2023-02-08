using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Enums
{
    public enum TranscriptJobStatusEnum
    {
        Created,
        SentToAuthenticity,
        Completed,
        Error,
        Processing,
        Canceled
    }
}
