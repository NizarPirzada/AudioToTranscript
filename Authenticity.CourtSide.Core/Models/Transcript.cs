using Authenticity.CourtSide.Core.Enums;
using System;

namespace Authenticity.CourtSide.Core.Models
{
    public class Transcript
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MediaFileSize { get; set; }
        public TranscriptStatusEnum Status { get; set; }
        public int Step { get; set; }
        public DateTime? RecordDate { get; set; }
        public string CaseNumber { get; set; }
        public string JudgeName { get; set; }
        public string JudgeTitle { get; set; }
        public int Type { get; set; }
        public TranscriptHumanTranscriptionStatusEnum HumanTranscriptionStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        // Additional Information
        public TranscriptFile TranscriptFile { get; set; }
        public CourtModel Court { get; set; } = new CourtModel();
        public TranscriptTypeEnum TranscriptType { get; set; }
        public string DeptNumber { get; set; }
        public int ApiLanguageId { get; set; }
        public TranscriptRecordingInfoModel TranscriptRecordingInfo { get; set; } = new TranscriptRecordingInfoModel();
		public bool Locked { get; set; }
    }
}
