using System;
using System.Threading;
using System.Threading.Tasks;

namespace Une.TalentPool.Configurations
{
    public class SettingValueManager : IDisposable
    {
        private bool _disposed;
        public SettingValueManager(ISettingValueStore settingValueStore)
        {
            SettingValueStore = settingValueStore;
        }
        protected CancellationToken CancellationToken => CancellationToken.None;
        protected ISettingValueStore SettingValueStore;
        public async Task<SettingValue> CreateAsync(SettingValue settingValue)
        {
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));

            return await SettingValueStore.CreateAsync(settingValue, CancellationToken);
        }

        public async Task<SettingValue> UpdateAsync(SettingValue settingValue)
        {
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));

            return await SettingValueStore.UpdateAsync(settingValue, CancellationToken);
        }

        public async Task<SettingValue> DeleteAsync(SettingValue settingValue)
        {
            if (settingValue == null)
                throw new ArgumentNullException(nameof(settingValue));

            return await SettingValueStore.DeleteAsync(settingValue, CancellationToken);
        }
        public async Task<SettingValue> FindByIdAsync(Guid settingValueId)
        {
            if (settingValueId == null)
                throw new ArgumentNullException(nameof(settingValueId));

            return await SettingValueStore.FindByIdAsync(settingValueId, CancellationToken);
        }
        public async Task<SettingValue> FindByNameAsync(string settingName)
        {
            if (settingName == null)
                throw new ArgumentNullException(nameof(settingName));

            return await SettingValueStore.FindByNameAsync(settingName, CancellationToken);
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
