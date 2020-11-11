using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentPool.Dictionaries;

namespace TalentPool.EntityFrameworkCore.Stores
{
    public class DictionaryStore : StoreBase, IDictionaryStore
    {
        public DictionaryStore(TalentDbContext context)
            : base(context)
        {
        } 
        public async Task<Dictionary> CreateAsync(Dictionary dictionary, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            Context.Dictionaries.Add(dictionary);
            await SaveChanges(cancellationToken);
            return dictionary;

        }
        public async Task<Dictionary> UpdateAsync(Dictionary dictionary, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            // 清除子实体集合防止更新异常
            var dictionaryValues = await Context.DictionaryItems.Where(w => w.DictionaryId == dictionary.Id).ToListAsync();
            if (dictionaryValues != null)
            {
                foreach (var dictionaryValue in dictionaryValues)
                {
                    Context.DictionaryItems.Remove(dictionaryValue);
                }
            }

            Context.Dictionaries.Update(dictionary); 

            await SaveChanges(cancellationToken);
            return dictionary;
        }
        public async Task<Dictionary> DeleteAsync(Dictionary dictionary, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            Context.Dictionaries.Remove(dictionary);
            await SaveChanges(cancellationToken);
            return dictionary;
        }


        public async Task<Dictionary> FindByIdAsync(Guid dictionaryId, CancellationToken cancellationToken)
        {
            return await Context.Dictionaries.Include(i => i.DictionaryItems).FirstOrDefaultAsync(f => f.Id == dictionaryId);
        }

        public async Task<Dictionary> FindByNameAsync(string dictionaryName, CancellationToken cancellationToken)
        {
            return await Context.Dictionaries.Include(i => i.DictionaryItems).FirstOrDefaultAsync(f => f.Name == dictionaryName);
        }
    }
}
