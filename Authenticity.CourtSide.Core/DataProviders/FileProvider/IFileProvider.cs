using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.DataProviders.FileProvider
{
    public interface IFileProvider
    {
        Task<string> SaveFileAsync(string sourceFilePath, string targetFileName);
        Task<string> GetFileToLocalTempDirectoryAsync(string ftpRoute, string destinationFileName);
        Task<FileStream> GetFileStreamAsync(string ftpRoute, string destinationFilePath);
        Task DeleteFileAsync(string fileName);
    }
}
