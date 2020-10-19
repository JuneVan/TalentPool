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

        public async Task AddSettingValueAsync(SettingValue settingValue, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));
            Context.SettingValues.Add(settingValue);
            await Task.CompletedTask;
        }

        public async Task ChangeSettingValueAsync(SettingValue settingValue, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));
            Context.SettingValues.Update(settingValue);
            await Task.CompletedTask;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            await SaveChanges(cancellationToken);
        }

        public async Task<SettingValue> FindByNameAsync(string settingName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await Context.SettingValues.FirstOrDefaultAsync(f => f.Name == settingName, cancellationToken);
        }

        public async Task<SettingValue> FindByOwnerUserIdAsync(Guid ownerUserId, string settingName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await Context.SettingValues.FirstOrDefaultAsync(f => f.Name == settingName && f.OwnerUserId == ownerUserId, cancellationToken);
        }
    }
}
