using Authenticity.CourtSide.Core.Enums;
using System;

namespace Authenticity.CourtSide.Core.Models
{
	public class TranscriptJob : Timestamp
    {
        public int Id { get; set; }
        public int TranscriptId { get; set; }
        public TranscriptJobStatusEnum Status { get; set; }
        public string JobGuid { get; set; }
        public string ApiUrl { get; set; }
		public string TranscriptionEngine { get; set; }

		public string Message { get; set; }
		public DateTime? StartSendingToApiOn { get; set; }
		public DateTime? SentToApiOn { get; set; }
		public DateTime? CompletedOn { get; set; }
	}
}
