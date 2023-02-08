using Authenticity.CourtSide.Core.Enums;

namespace Authenticity.CourtSide.Core.Models
{
	public class ApiLanguage : Timestamp
	{
		public int ApiLanguageId { get; set; }
		public LanguageTypeEnum LanguageType { get; set; }
		public string Name { get; set; }
		public string ApiCode { get; set; }
		public string GenericCode { get; set; }
	}
}
