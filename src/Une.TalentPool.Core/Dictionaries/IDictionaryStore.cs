using System;
using System.Threading;
using System.Threading.Tasks;

namespace Une.TalentPool.Dictionaries
{
    public interface IDictionaryStore : IDisposable
    {
        Task<Dictionary> FindByIdAsync(Guid dictionaryId, CancellationToken cancellationToken);
        Task<Dictionary> FindByNameAsync(string dictionaryName, CancellationToken cancellationToken);
        Task<Dictionary> CreateAsync(Dictionary dictionary, CancellationToken cancellationToken);
        Task<Dictionary> UpdateAsync(Dictionary dictionary, CancellationToken cancellationToken);
        Task<Dictionary> DeleteAsync(Dictionary dictionary, CancellationToken cancellationToken);

    }
}
