using Authenticity.CourtSide.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Authenticity.CourtSide.WebApp.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private int? _userId;

        public int UserId
        {
            get
            {
                if (!_userId.HasValue)
                {
                    string id = User.FindFirst(CourtsideClaimTypes.Id).Value;
                    _userId = int.Parse(id, CultureInfo.InvariantCulture);
                }

                return _userId.GetValueOrDefault();
            }
        }
    }
}
