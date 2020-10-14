using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.Configurations
{
    public interface ISettingValueStore : IDisposable
    {
        Task<SettingValue> CreateAsync(SettingValue setting, CancellationToken cancellationToken);
        Task<SettingValue> UpdateAsync(SettingValue setting, CancellationToken cancellationToken);
        Task<SettingValue> DeleteAsync(SettingValue setting, CancellationToken cancellationToken);
        Task<SettingValue> FindByIdAsync(Guid settingId, CancellationToken cancellationToken);
        Task<SettingValue> FindByNameAsync(string settingName, CancellationToken cancellationToken);
    }
}
