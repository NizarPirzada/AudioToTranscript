namespace Authenticity.CourtSide.Core.Models
{
	public class TranscriptRecordingInfoModel: Timestamp
	{
		public int? TranscriptRecordingInfoId { get; set; }
		public string VenueCompany { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zipcode { get; set; }
		public int TranscriptId { get; set; }
	}
}
