using Authenticity.CourtSide.Core.Models;
using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Domains
{
    public interface IEmailDomain
    {
        void SendUserRegisterEmail(UserModel userModel);
        void SendPasswordRecoveryEmail(UserModel userModel, string temporaryPassword);
    }
}
