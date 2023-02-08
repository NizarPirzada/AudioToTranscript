using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface ISettingsQueryRepository
    {
        Task<IEnumerable<FileProviderModel>> GetAllFileProvidersAsync();
        Task<FileProviderModel> GetCurrentFileProviderAsync();
        Task<FileProviderModel> GetFileProviderByIdAsync(int fileProviderId);
    }
}
