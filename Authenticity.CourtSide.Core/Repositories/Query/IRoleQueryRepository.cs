using Authenticity.CourtSide.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query
{
    public interface IRoleQueryRepository
    {
        Task<IEnumerable<RoleModel>> GetAllRolesAsync();
        Task<IEnumerable<RoleModel>> GetAllRolesWithObjectPermissionsAsync();
    }
}
