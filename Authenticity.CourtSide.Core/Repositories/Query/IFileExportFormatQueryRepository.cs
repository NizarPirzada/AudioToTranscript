using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface IFileExportFormatQueryRepository
    {
        Task<FileExportFormat> GetExportFormatByIdAsync(int formatId);
        Task<FileExportFormat> GetFormatByNameAndTypeAsync(string formatName, string type);
    }
}
