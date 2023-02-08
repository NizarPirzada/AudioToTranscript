using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
	public class LanguageQueryRepository : ILanguageQueryRepository
    {
        private IDapperBase<ApiLanguage> DapperLanguage { get; set; }

        public LanguageQueryRepository(IDapperBase<ApiLanguage> dapperLanguage)
        {
            DapperLanguage = dapperLanguage;
        }

		public Task<ApiLanguage> GetLanguageByIdAsync(int languageId)
		{
			var parameters = new { languageId };
			return DapperLanguage.GetByIdAsync(SqlStatement.Language_GetById, parameters);
		}

		public Task<IEnumerable<ApiLanguage>> GetAllLanguagesByTypeAsync(LanguageTypeEnum languageType)
		{
			var parameters = new { languageType };
			return DapperLanguage.GetAllAsync(SqlStatement.Language_GetByType, parameters);
		}
	}
}
