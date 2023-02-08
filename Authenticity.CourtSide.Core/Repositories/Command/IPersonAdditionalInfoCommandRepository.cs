using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface IPersonAdditionalInfoCommandRepository
    {
        Task<int> SaveAdditionalInfoAsync(PersonAdditionalInformation additionalInfoDto);
        Task<int> UpdateAdditionalInfoAsync(PersonAdditionalInformation additionalInfoDto);
        Task<int> DeleteAdditionalInfoAsync(int personAdditionalInformationId);
    }
}
