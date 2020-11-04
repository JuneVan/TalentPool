using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TalentPool.Infrastructure.Notify
{
    public class Notifier : INotifier
    {
        public Notifier(ILogger<Notifier> logger)
        {
            Logger = logger;
            _notifyEntries = new List<NotifyEntry>();
        }
        public ILogger Logger { get; set; }
        private List<NotifyEntry> _notifyEntries;
        public NotifyEntry[] NotifyEntries => _notifyEntries.ToArray();

        public void Add(NotifyType type, string message)
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Notification '{NotificationType}' with message '{NotificationMessage}'", type, message);
            }
            _notifyEntries.Add(new NotifyEntry { Type = type, Message = message });
        }

    }
}
