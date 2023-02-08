using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
	public class FileExportFormatQueryRepository : IFileExportFormatQueryRepository
    {
        private IDapperBase<FileExportFormat> ExportFormat { get; set; }

        public FileExportFormatQueryRepository(IDapperBase<FileExportFormat> exportFormat)
        {
            ExportFormat = exportFormat;
        }

        public Task<FileExportFormat> GetExportFormatByIdAsync(int formatId)
        {
            var parameters = new { formatId };
            var formatResult = ExportFormat.GetByIdAsync(SqlStatement.FileFormat_GetFormatById, parameters);
            return formatResult;
        }

        public Task<FileExportFormat> GetFormatByNameAndTypeAsync(string formatName, string type)
        {
	        var parameters = new { formatName, type };
	        return ExportFormat.GetByIdAsync(SqlStatement.FileFormat_GetByFormatNameAndType, parameters);
        }
    }
}
