using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ILanguageQueryRepository
    {
        Task<ApiLanguage> GetLanguageByIdAsync(int languageId);
        Task<IEnumerable<ApiLanguage>> GetAllLanguagesByTypeAsync(LanguageTypeEnum languageTypeEnum);
    }
}
