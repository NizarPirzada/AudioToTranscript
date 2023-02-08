using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Threading.Tasks;
using Authenticity.CourtSide.Core.Entities;

namespace Authenticity.CourtSide.Core.Repositories.Command.Implementation
{
    public class SettingsCommandRepository : ISettingsCommandRepository
    {
        public SettingsCommandRepository(IDapperBase<FileProviderModel> dapperFileProvider)
        {
            DapperFileProvider = dapperFileProvider;
        }

        private IDapperBase<FileProviderModel> DapperFileProvider { get; set; }

        public Task<int> CreateFileProviderAsync(CreateFileProviderDto createFileProviderDto)
        {
            return DapperFileProvider.SaveAsync(SqlStatement.FileProvider_CreateFileProvider, createFileProviderDto);
        }

        public Task<int> UpdateFileProviderAsync(UpdateFileProviderDto updateFileProviderDto)
        {
            return DapperFileProvider.EditAsync(SqlStatement.FileProvider_UpdateFileProvider, updateFileProviderDto);
        }
    }
}
