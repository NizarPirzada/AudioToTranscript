using Authenticity.CourtSide.Core.Entities;
using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Command
{
    public interface ISettingsCommandRepository
    {
        Task<int> CreateFileProviderAsync(CreateFileProviderDto createFileProviderDto);
        Task<int> UpdateFileProviderAsync(UpdateFileProviderDto updateFileProviderDto);
    }
}
