using System;
using System.Threading;
using System.Threading.Tasks;

namespace TalentPool.Configurations
{
    public interface ISettingValueStore : IDisposable
    { 
        Task<SettingValue> FindByNameAsync(string settingName, CancellationToken cancellationToken);
        Task<SettingValue> FindByOwnerUserIdAsync(Guid ownerUserId, string settingName, CancellationToken cancellationToken);

        Task AddSettingValueAsync(SettingValue settingValue, CancellationToken cancellationToken);
        Task ChangeSettingValueAsync(SettingValue settingValue, CancellationToken cancellationToken);
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
