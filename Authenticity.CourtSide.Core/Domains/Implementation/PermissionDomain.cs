using Authenticity.CourtSide.Core.Models;
using Authenticity.CourtSide.Core.Repositories.Query;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains.Implementation
{
	public class PermissionDomain : IPermissionDomain
    {
        private IRoleQueryRepository RoleQueryRepository { get; }
        private IMemoryCache MemoryCache { get; }
        private MemoryCacheConfiguration MemoryCacheConfiguration { get; }
		private ITranscriptQueryRepository TranscriptQueryRepository { get; }

		public PermissionDomain(IRoleQueryRepository roleQueryRepository, IMemoryCache memoryCache, MemoryCacheConfiguration memoryCacheConfiguration, ITranscriptQueryRepository transcriptQueryRepository)
        {
            RoleQueryRepository = roleQueryRepository;
            MemoryCache = memoryCache;
            MemoryCacheConfiguration = memoryCacheConfiguration;
			TranscriptQueryRepository = transcriptQueryRepository;
		}

        public async Task<IEnumerable<ObjectModel>> GetObjectPermissionByUserAsync(UserModel userRequest)
        {
            var userRoles = userRequest.Roles.Select(x => x.Name);

            var rolesWithPermissions = await GetAllRolesWithObjectPermissions();

            HashSet<ObjectModel> objectResponse = new HashSet<ObjectModel>();

            foreach (var role in rolesWithPermissions)
            {
                if (userRoles.Contains(role.Name))
                {
                    foreach (var objectPermission in role.ObjectPermissions)
                    {
                        ObjectModel obj = objectResponse.Where(s => s.Id == objectPermission.ObjectId).FirstOrDefault();

                        if (obj != null)
                        {
                            obj.Permissions.Add(new PermissionModel { PermissionId = objectPermission.PermissionId, Name = objectPermission.PermissionName });
                        }
                        else
                        {
                            obj = new ObjectModel { Id = objectPermission.ObjectId, Name = objectPermission.ObjectName };
                            obj.Permissions.Add(new PermissionModel { PermissionId = objectPermission.PermissionId, Name = objectPermission.PermissionName });
                            objectResponse.Add(obj);
                        }
                    }
                }
            }

            return objectResponse;
        }

        public async Task<bool> CheckUserPermissionAsync(UserModel userRequest, string permission)
        {
            var userRoles = userRequest.Roles.Select(x => x.Name);
            var objectPermissions = await GetAllRolesWithObjectPermissions();

            var permissionsRole = objectPermissions
                                    .Where(x => userRoles.Contains(x.Name))
                                    .SelectMany(x => x.ObjectPermissions.Select(y => $"{y.PermissionName}{y.ObjectName}"))
                                    .Distinct();

            var hasClaim = permissionsRole.Any(p => permission == p);

            return hasClaim;
        }

        public Task<bool> CheckTranscriptPermissionAsync(int userId, int transcriptId)
		{
            return TranscriptQueryRepository.ChekUserTranscriptAccessAsync(userId, transcriptId);
        }

        private async Task<IEnumerable<RoleModel>> GetAllRolesWithObjectPermissions()
        {
            return await MemoryCache.GetOrCreateAsync(MemoryCacheConfiguration.RolesKey, cacheEntry =>
            {
                cacheEntry.AbsoluteExpiration = DateTime.Now.AddMinutes(MemoryCacheConfiguration.ExpirationTimeInMinutes);
                cacheEntry.Priority = CacheItemPriority.Normal;
                return RoleQueryRepository.GetAllRolesWithObjectPermissionsAsync();
            });
        }
    }
}
