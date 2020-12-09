using System.Threading;

namespace TalentPool
{
    public interface ISignal
    {
        CancellationToken Token { get; }
    }
}
