using System.Threading.Tasks;

namespace Authenticity.CourtSide.Core.Utilities.SignarlR
{
	public interface ITranscriptionNotificationHub
	{
		Task NotifyExaminationSavingStatusAsync(string userId, string message, bool success = true);
	}
}
