using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Dictionaries
{
    public class DictionaryManager : ObjectDisposable
    {
        public DictionaryManager(IDictionaryStore dictionaryStore,
            IOptions<DictionaryOptions> options,
            ISignal signal)
        {
            DictionaryStore = dictionaryStore;
            Signal = signal;
            if (options.Value.Injects != null)
            {
                InjectDictionaries = options.Value.Injects.ToList();
            }
        }
        protected ISignal Signal { get; }
        public IReadOnlyList<Dictionary> InjectDictionaries { get; }
        protected IDictionaryStore DictionaryStore { get; }
        protected virtual CancellationToken CancellationToken => Signal.Token;

        public async Task<Dictionary> CreateAsync(Dictionary dictionary)
        {
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return await DictionaryStore.CreateAsync(dictionary, CancellationToken);
        }

        public async Task<Dictionary> UpdateAsync(Dictionary dictionary)
        {
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            await DictionaryStore.UpdateAsync(dictionary, CancellationToken);
            return dictionary;
        }

        public async Task<Dictionary> DeleteAsync(Dictionary dictionary)
        {
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return await DictionaryStore.DeleteAsync(dictionary, CancellationToken);
        }

        public async Task<Dictionary> FindByIdAsync(Guid dictionaryId)
        {
            ThrowIfDisposed();
            return await DictionaryStore.FindByIdAsync(dictionaryId, CancellationToken);
        }

    }
}
