using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.ORM;
using Authenticity.CourtSide.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Repositories.Query.Implementation
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private IDapperBase<UserModel> DapperUser { get; set; }

        public UserQueryRepository(IDapperBase<UserModel> dapperUser)
        {
            DapperUser = dapperUser;
        }

        public async Task<UserModel> GetUserByIdAsync(int userId)
        {
            var parameters = new { userId };
            IEnumerable<UserModel> userResult = await DapperUser.GetByIdWithRelationsAsync<RoleModel>(SqlStatement.User_GetUserById, MapUserRelations, "RoleId", parameters);
            return userResult.FirstOrDefault();
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var parameters = new { email };
            IEnumerable<UserModel> userResult = await DapperUser.GetByIdWithRelationsAsync<RoleModel>(SqlStatement.User_GetUserByEmail, MapUserRelations, "RoleId", parameters);
            return userResult.FirstOrDefault();
        }

        public Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return DapperUser.QueryAllWithRelationsAsync<RoleModel>(SqlStatement.User_GetAllUsers, MapUserRelations, "RoleId");
        }

        public async Task<UserModel> GetUserByEmailActivationIdAsync(string emailActivationId)
        {
            var parameters = new { emailActivationId };
            IEnumerable<UserModel> userResult = await DapperUser.GetByIdWithRelationsAsync<RoleModel>(SqlStatement.User_GetUserByEmailActivationId, MapUserRelations, "RoleId", parameters);
            return userResult.FirstOrDefault();
        }

        public Task<bool> CheckTemporalPasswordAsync(int userId, string temporalPassword)
		{
            var parameters = new { userId, temporalPassword };
            return DapperUser.GetAsync<bool>(SqlStatement.User_CheckTemporalPassword, parameters);
        }

        private UserModel MapUserRelations(UserModel userModel, RoleModel roleModel, IDictionary<int, UserModel> userRelations)
        {
            UserModel userRow = ValidateDictionaryKey(userModel, userRelations, userModel.Id);
            MapRoles(roleModel, userRow);
            return userRow;
        }

        private void MapRoles(RoleModel roleModel, UserModel userRow)
        {
            if (roleModel != null)
            {
                bool isRoleAdded = userRow.Roles.Any(rol => rol.RoleId == roleModel.RoleId);
                if (!isRoleAdded)
                {
                    userRow.Roles.Add(roleModel);
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
