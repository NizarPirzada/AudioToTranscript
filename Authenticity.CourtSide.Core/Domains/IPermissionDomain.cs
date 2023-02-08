using Authenticity.CourtSide.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface IPermissionDomain : IDomain
    {
		/// <summary>
		/// Get All objects linked to the user, including its permissions.
		/// </summary>
		/// <param name="userId">.</param>
		/// <returns>.</returns>
		Task<IEnumerable<ObjectModel>> GetObjectPermissionByUserAsync(UserModel userRequest);

		Task<bool> CheckUserPermissionAsync(UserModel userRequest, string permission);
		/// <summary>
		/// Method to check permissions on a transcript
		/// </summary>
		/// <param name="userId">User Id</param>
		/// <param name="transcriptId">Transcript Id</param>
		/// <returns>True or False</returns>
		Task<bool> CheckTranscriptPermissionAsync(int userId, int transcriptId);
	}
}
