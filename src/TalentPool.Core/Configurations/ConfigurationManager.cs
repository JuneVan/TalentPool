using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TalentPool.Configurations
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
        public async Task<TSettingDefinition> GetSettingAsync<TSettingDefinition>(Guid? ownerUserId = null)
            where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = GetSettingTypeName<TSettingDefinition>(ownerUserId);
            return await _memoryCache.GetOrCreateAsync(settingTypeName, async factory =>
            {
                var settings = Activator.CreateInstance<TSettingDefinition>();
                foreach (var propertyInfo in typeof(TSettingDefinition).GetProperties())
                {
                    var settingName = GetSettingName<TSettingDefinition>(propertyInfo.Name);
                    SettingValue settingValue;
                    if (ownerUserId.HasValue)
                        settingValue = await _settingValueManager.FindByOwnerUserIdAsync(ownerUserId.Value, settingName);
                    else
                        settingValue = await _settingValueManager.FindByNameAsync(settingName);
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


        public async Task SaveSettingAsync<TSettingDefinition>(TSettingDefinition settings, Guid? ownerUserId = null)
             where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = GetSettingTypeName<TSettingDefinition>(ownerUserId);

            var settingValues = new List<SettingValue>();
            foreach (var propertyInfo in typeof(TSettingDefinition).GetProperties())
            {
                if (propertyInfo.Name == "OwnerUserId")
                    continue;
                var settingName = GetSettingName<TSettingDefinition>(propertyInfo.Name);
                var propertyValue = propertyInfo.GetValue(settings);
                SettingValue settingValue;
                if (ownerUserId.HasValue)
                    settingValue = await _settingValueManager.FindByOwnerUserIdAsync(ownerUserId.Value, settingName);
                else
                    settingValue = await _settingValueManager.FindByNameAsync(settingName);

                if (settingValue == null)
                {
                    settingValues.Add(new SettingValue()
                    {
                        Name = settingName,
                        Value = Convert.ToString(propertyValue),
                        OwnerUserId = ownerUserId
                    });
                }
                else
                {
                    settingValue.Value = Convert.ToString(propertyValue);
                    settingValues.Add(settingValue);
                }
            }
            await _settingValueManager.BulkAsync(settingValues);
            _memoryCache.Remove(settingTypeName);
        }

        public async Task<TSettingPropertyType> GetSettingValueAsync<TSettingDefinition, TSettingPropertyType>(string propertyName, TSettingPropertyType defaultValue, Guid? ownerUserId = null)
             where TSettingDefinition : ISettingDefinition
        {
            var settingTypeName = GetSettingTypeName<TSettingDefinition>(ownerUserId);
            var settingName = GetSettingName<TSettingDefinition>(propertyName);
            SettingValue settingValue;
            if (ownerUserId.HasValue)
                settingValue = await _settingValueManager.FindByOwnerUserIdAsync(ownerUserId.Value, settingName);
            else
                settingValue = await _settingValueManager.FindByNameAsync(settingName);
            if (settingValue != null)
                return (TSettingPropertyType)Convert.ChangeType(settingValue.Value, typeof(TSettingPropertyType));
            return defaultValue;
        }

        private string GetSettingName<TSettingDefinition>(string propertyName)
             where TSettingDefinition : ISettingDefinition
        {
            return $"{typeof(TSettingDefinition).FullName}.{propertyName}";
        }
        private string GetSettingTypeName<TSettingDefinition>(Guid? ownerUserId)
             where TSettingDefinition : ISettingDefinition
        {
            if (ownerUserId.HasValue)
                return $"{typeof(TSettingDefinition).FullName}.{ownerUserId.Value}";
            return typeof(TSettingDefinition).FullName;
        }
    }
}
