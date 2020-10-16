using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using System.Net;

namespace Van.TalentPool.Infrastructure.Notify
{
    public class NotifySerializer : INotifySerializer
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        public NotifySerializer(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
        public NotifyEntry[] Deserialize(string value)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NotifyEntryConverter());
            NotifyEntry[] messageEntries;
            try
            {
                var protector = _dataProtectionProvider.CreateProtector(nameof(NotifyFilter));
                var decoded = protector.Unprotect(WebUtility.UrlDecode(value));
                messageEntries = JsonConvert.DeserializeObject<NotifyEntry[]>(decoded, settings);
            }
            catch
            {
                messageEntries = null;
            }
            return messageEntries;
        }

        public string Serialize(NotifyEntry[] notifyEntries)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NotifyEntryConverter());

            try
            {
                var protector = _dataProtectionProvider.CreateProtector(nameof(NotifyFilter));
                var signed = protector.Protect(JsonConvert.SerializeObject(notifyEntries, settings));
                return WebUtility.UrlEncode(signed);
            }
            catch
            {
                return null;
            }
        }
    }
}
