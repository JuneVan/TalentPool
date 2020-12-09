using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Dictionaries
{
    public class DictionaryManager : IDisposable
    {
        private bool _disposed;
        private readonly ISignal _signal;
        public DictionaryManager(IDictionaryStore dictionaryStore,
            IOptions<DictionaryOptions> options,
            ISignal  signal)
        {
            DictionaryStore = dictionaryStore;
            _signal = signal;
            if (options.Value.Injects != null)
            {
                InjectDictionaries = options.Value.Injects.ToList();
            }

        }
        public IReadOnlyList<Dictionary> InjectDictionaries { get; }
        protected IDictionaryStore DictionaryStore { get; }
        protected virtual CancellationToken CancellationToken => _signal.Token;

        public async Task<Dictionary> CreateAsync(Dictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return await DictionaryStore.CreateAsync(dictionary, CancellationToken);
        }

        public async Task<Dictionary> UpdateAsync(Dictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            await DictionaryStore.UpdateAsync(dictionary, CancellationToken);
            return dictionary;
        }

        public async Task<Dictionary> DeleteAsync(Dictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return await DictionaryStore.DeleteAsync(dictionary, CancellationToken);
        }

        public async Task<Dictionary> FindByIdAsync(Guid dictionaryId)
        {
            return await DictionaryStore.FindByIdAsync(dictionaryId, CancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                DictionaryStore.Dispose();
            }
            _disposed = true;
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
