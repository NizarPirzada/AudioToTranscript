using Authenticity.CourtSide.Core.Enums;
using System;

namespace Authenticity.CourtSide.Core.Entities
{
	public class TranscriptionStatus
	{
		public int TranscriptId { get; set; }
		public TranscriptJobStatusEnum Status { get; set; }
		public string Message { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? StartSendingOn { get; set; }
		public DateTime? SentOn { get; set; }
		public DateTime? CompletedOn { get; set; }
	}
}
