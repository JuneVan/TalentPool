using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Configurations
{
    public class SettingValueManager : ObjectDisposable
    {
        public SettingValueManager(ISettingValueStore settingValueStore,
            ISignal signal)
        {
            SettingValueStore = settingValueStore;
            Signal = signal;
        }
        protected ISignal Signal { get; }
        protected CancellationToken CancellationToken => Signal.Token;
        protected ISettingValueStore SettingValueStore;

        public async Task<SettingValue> FindByNameAsync(string settingName)
        {
            ThrowIfDisposed();
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await SettingValueStore.FindByNameAsync(settingName, CancellationToken);
        }
        public async Task<SettingValue> FindByOwnerUserIdAsync(Guid userId, string settingName)
        {
            ThrowIfDisposed();
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await SettingValueStore.FindByOwnerUserIdAsync(userId, settingName, CancellationToken);
        }
        public async Task BulkAsync(List<SettingValue> settingValues)
        {
            ThrowIfDisposed();
            if (settingValues == null)
                throw new ArgumentNullException(nameof(settingValues));
            foreach (var settingValue in settingValues)
            {
                if (settingValue.Id == Guid.Empty)
                    await SettingValueStore.AddSettingValueAsync(settingValue, CancellationToken);
                else
                    await SettingValueStore.ChangeSettingValueAsync(settingValue, CancellationToken);
            }
            await SettingValueStore.CommitAsync(CancellationToken);
        }
       
    }
}
