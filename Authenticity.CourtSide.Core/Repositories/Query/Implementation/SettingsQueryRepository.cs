using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class SettingsQueryRepository: ISettingsQueryRepository
    {
        public SettingsQueryRepository(IDapperBase<FileProviderModel> dapperFileProvider)
        {
            DapperFileProvider = dapperFileProvider;
        }

        private IDapperBase<FileProviderModel> DapperFileProvider { get; set; }

        public Task<IEnumerable<FileProviderModel>> GetAllFileProvidersAsync()
        {
            return DapperFileProvider.GetAllAsync(SqlStatement.FileProvider_GetAllFileProviders);
        }

        public Task<FileProviderModel> GetCurrentFileProviderAsync()
        {
            return DapperFileProvider.GetByIdAsync(SqlStatement.FileProvider_GetCurrentFileProvider);
        }

        public Task<FileProviderModel> GetFileProviderByIdAsync(int fileProviderId)
        {
            var parameters = new {fileProviderId};
            return DapperFileProvider.GetByIdAsync(SqlStatement.FileProvider_GetFileProviderById, parameters);
        }
    }
}
