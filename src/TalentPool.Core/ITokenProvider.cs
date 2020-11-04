using System.Threading;

namespace TalentPool
{
    public interface ITokenProvider
    {
        CancellationToken Token { get; }
    }
}
