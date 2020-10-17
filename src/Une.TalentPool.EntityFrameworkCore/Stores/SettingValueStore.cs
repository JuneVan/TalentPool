using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Configurations;

namespace Une.TalentPool.EntityFrameworkCore.Stores
{
    public class SettingValueStore : StoreBase, ISettingValueStore
    {
        public SettingValueStore(VanDbContext context) : base(context)
        {

        }
        public async Task<SettingValue> CreateAsync(SettingValue settingValue, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));
            Context.SettingValues.Add(settingValue);
            await SaveChanges(cancellationToken);
            return settingValue;
        }
        public async Task<SettingValue> UpdateAsync(SettingValue settingValue, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));
            Context.SettingValues.Update(settingValue);
            await SaveChanges(cancellationToken);
            return settingValue;
        }
        public async Task<SettingValue> DeleteAsync(SettingValue settingValue, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));
            Context.SettingValues.Remove(settingValue);
            await SaveChanges(cancellationToken);
            return settingValue;
        }

        public async Task<SettingValue> FindByIdAsync(Guid settingId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingId == null)
                throw new ArgumentNullException(nameof(settingId));

            return await Context.SettingValues.FirstOrDefaultAsync(f => f.Id == settingId, cancellationToken);
        }

        public async Task<SettingValue> FindByNameAsync(string settingName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await Context.SettingValues.FirstOrDefaultAsync(f => f.Name == settingName, cancellationToken);
        }


    }
}
