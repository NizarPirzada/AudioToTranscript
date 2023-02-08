using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class RoleQueryRepository : IRoleQueryRepository
    {
        private IDapperBase<RoleModel> DapperContext { get; set; }

        public RoleQueryRepository(IDapperBase<RoleModel> dapperContext)
        {
            DapperContext = dapperContext;
        }

        public Task<IEnumerable<RoleModel>> GetAllRolesWithObjectPermissionsAsync()
        {
            return DapperContext.GetByIdWithRelationsAsync<ObjectPermissionModel>(SqlStatement.Permission_GetAllRolesWithObjectPermission, MapObjectRelations, "ObjectPermissionId");
        }

        public Task<IEnumerable<RoleModel>> GetAllRolesAsync()
        {
            return DapperContext.GetAllAsync(SqlStatement.Role_GetAllRoles);
        }

        private RoleModel MapObjectRelations(RoleModel roleModel, ObjectPermissionModel objectPermissionModel, IDictionary<int, RoleModel> roleRelations)
        {
            RoleModel roleRow = ValidateDictionaryKey(roleModel, roleRelations, roleModel.RoleId);
            MapPermissions(objectPermissionModel, roleRow);
            return roleRow;
        }

        private void MapPermissions(ObjectPermissionModel objectPermissionModel, RoleModel roleRow)
        {
            if (objectPermissionModel != null)
            {
                bool isPermissionAdded = roleRow.ObjectPermissions.Any(s => s.ObjectPermissionId == objectPermissionModel.ObjectPermissionId);
                if (!isPermissionAdded)
                {
                    roleRow.ObjectPermissions.Add(objectPermissionModel);
                }
            }
        }

        private T ValidateDictionaryKey<T>(T model, IDictionary<int, T> relations, int id)
        {
            if (!relations.TryGetValue(id, out T item))
            {
                relations.Add(id, item = model);
            }

            return item;
        }
    }
}
