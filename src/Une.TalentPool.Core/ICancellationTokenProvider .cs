using System.Threading;

namespace Une.TalentPool
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
    }
}
