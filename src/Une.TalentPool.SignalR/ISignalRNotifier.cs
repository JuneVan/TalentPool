using System.Threading.Tasks;

namespace Une.TalentPool.SignalR
{
    public interface ISignalRNotifier
    {
        Task NotifyAsync(NotifyEntry notifyEntry);
    }
}
