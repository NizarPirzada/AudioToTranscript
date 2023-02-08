using Authenticity.CourtSide.Core.Enums;
using NSerio.Utils;
using System;

namespace Authenticity.CourtSide.Core.Models
{
    public class TranscriptPerson
    {
        public int Id { get; set; }
        public int TranscriptId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public PersonType Type { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public PersonAdditionalInformation AdditionalInfo { get; set; } = new PersonAdditionalInformation();

        public int? PersonAdditionalInformationId => AdditionalInfo.PersonAdditionalInformationId;
        public string FullName
        {
            get
            {
                return LastName.HasValue() ? $"{FirstName} {LastName}" : FirstName;
            }
        }
    }
}
