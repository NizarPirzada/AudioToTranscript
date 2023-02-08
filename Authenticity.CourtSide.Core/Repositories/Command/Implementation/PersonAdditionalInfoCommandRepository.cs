using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class PersonAdditionalInfoCommandRepository : IPersonAdditionalInfoCommandRepository
    {
        public PersonAdditionalInfoCommandRepository(IDapperBase<PersonAdditionalInformation> dapperInfo)
        {
            DapperInfo = dapperInfo;
        }

        private IDapperBase<PersonAdditionalInformation> DapperInfo { get; }

        public Task<int> SaveAdditionalInfoAsync(PersonAdditionalInformation additionalInfoDto)
        {
            var result = DapperInfo.SaveAsync(SqlStatement.PersonInformation_Create, additionalInfoDto);
            return result;
        }

        public Task<int> UpdateAdditionalInfoAsync(PersonAdditionalInformation additionalInfoDto)
        {
            var result = DapperInfo.EditAsync(SqlStatement.PersonInformation_Update, additionalInfoDto);
            return result;
        }

        public Task<int> DeleteAdditionalInfoAsync(int personAdditionalInformationId)
        {
            var parameters = new { personAdditionalInformationId };
            return DapperInfo.DeleteAsync(SqlStatement.PersonInformation_Delete, parameters);
        }
    }
}
