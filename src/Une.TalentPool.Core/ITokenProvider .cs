using System.Threading;

namespace Une.TalentPool
{
    public interface ITokenProvider
    {
        CancellationToken Token { get; }
    }
}
