using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Van.TalentPool.Configurations
{
    public class ConfigurationManager
    {
        private readonly SettingValueManager _settingValueManager;
        private readonly IMemoryCache _memoryCache;
        public ConfigurationManager(SettingValueManager settingValueManager,
            IMemoryCache memoryCache)
        {
            _settingValueManager = settingValueManager;
            _memoryCache = memoryCache;
        }
        public async Task<TSettingDefinition> GetSettingAsync<TSettingDefinition>()
            where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = typeof(TSettingDefinition).FullName;
            return await _memoryCache.GetOrCreateAsync(settingTypeName, async factory =>
            {
                var settings = Activator.CreateInstance<TSettingDefinition>();
                foreach (var propertyInfo in typeof(TSettingDefinition).GetProperties())
                {
                    var settingName = GetSettingName(settingTypeName, propertyInfo.Name);
                    var settingValue = await _settingValueManager.FindByNameAsync(settingName);
                    if (settingValue != null)
                    {
                        object propertyValue;
                        switch (propertyInfo.PropertyType.Name)
                        {
                            case "TimeSpan":
                                propertyValue = TimeSpan.Parse(settingValue.Value);
                                propertyInfo.SetValue(settings, propertyValue);
                                break;
                            default:
                                propertyValue = Convert.ChangeType(settingValue.Value, propertyInfo.PropertyType);
                                propertyInfo.SetValue(settings, propertyValue);
                                break;
                        }

                    }
                }
                return settings;
            });
        }

        public async Task SaveSettingAsync<TSettingDefinition>(TSettingDefinition settings)
             where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = typeof(TSettingDefinition).FullName;
            foreach (var propertyInfo in typeof(TSettingDefinition).GetProperties())
            {
                var settingName = GetSettingName(settingTypeName, propertyInfo.Name);
                var propertyValue = propertyInfo.GetValue(settings);
                var settingValue = await _settingValueManager.FindByNameAsync(settingName);
                if (settingValue == null)
                {
                    await _settingValueManager.CreateAsync(new SettingValue()
                    {
                        Name = settingName,
                        Value = Convert.ToString(propertyValue)
                    });
                }
                else
                {
                    settingValue.Value = Convert.ToString(propertyValue);
                    await _settingValueManager.UpdateAsync(settingValue);
                }
            }
            _memoryCache.Remove(settingTypeName);
        }

        public async Task<TSettingPropertyType> GetSettingValueAsync<TSettingDefinition, TSettingPropertyType>(string propertyName, TSettingPropertyType defaultValue)
             where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = typeof(TSettingDefinition).FullName;
            var settingName = GetSettingName(settingTypeName, propertyName);
            var settingValue = await _settingValueManager.FindByNameAsync(settingName);
            if (settingValue != null)
                return (TSettingPropertyType)Convert.ChangeType(settingValue.Value, typeof(TSettingPropertyType));
            return defaultValue;
        }
        private string GetSettingName(string settingTypeName, string propertyName)
        {
            return $"{settingTypeName}.{propertyName}";
        }
    }
}
