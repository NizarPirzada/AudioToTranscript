using Authenticity.CourtSide.Core.Models;
using System.IO;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface IFileDomain : IDomain
    {
        Task<string> SaveFileTempLocationAsync(TranscriptFile file);
        Task<bool> DeleteFileTempLocationAsync(string filePath);

        Task<string> SaveFileToFTPAsync(int transcriptId, decimal size, string fileName);
        Task<FileStream> GetAudioFileAsync(int transcriptId);
    }
}
