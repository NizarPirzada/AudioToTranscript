using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface ISettingsDomain
    {
        Task<IEnumerable<FileProviderModel>> GetAllFileProvidersAsync();
        Task<FileProviderModel> CreateFileProviderAsync(CreateFileProviderDto createFileProviderDto);
        Task<FileProviderModel> UpdateFileProviderAsync(UpdateFileProviderDto updateFileProviderDto);
        Task<FileProviderModel> GetCurrentFileProviderAsync();
    }
}
