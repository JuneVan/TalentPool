using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Configurations
{
    public class SettingValueManager : IDisposable
    {
        private bool _disposed;
        private readonly ISignal _signal;
        public SettingValueManager(ISettingValueStore settingValueStore,
            ISignal  signal)
        {
            SettingValueStore = settingValueStore;
            _signal = signal;
        }
        protected CancellationToken CancellationToken => _signal.Token;
        protected ISettingValueStore SettingValueStore;

        public async Task<SettingValue> FindByNameAsync(string settingName)
        {
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await SettingValueStore.FindByNameAsync(settingName, CancellationToken);
        }
        public async Task<SettingValue> FindByOwnerUserIdAsync(Guid userId, string settingName)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await SettingValueStore.FindByOwnerUserIdAsync(userId, settingName, CancellationToken);
        }
        public async Task BulkAsync(List<SettingValue> settingValues)
        {
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                SettingValueStore.Dispose();
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
