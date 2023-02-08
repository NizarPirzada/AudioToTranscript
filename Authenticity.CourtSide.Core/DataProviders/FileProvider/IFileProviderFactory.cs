using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders.FileProvider
{
    public interface IFileProviderFactory
    {
        Task<IFileProvider> CreateAsync();
    }
}
