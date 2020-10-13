using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Van.TalentPool.Infrastructure.Notify
{
    public class Notifier : INotifier
    {
        private readonly IList<NotifyEntry> _entries;

        public Notifier(ILogger<Notifier> logger)
        {
            Logger = logger;
            _entries = new List<NotifyEntry>();
        }
        public ILogger Logger { get; set; }

        public void Add(NotifyType type, string message)
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Notification '{NotificationType}' with message '{NotificationMessage}'", type, message);
            }

            _entries.Add(new NotifyEntry { Type = type, Message = message });
        }

        public IList<NotifyEntry> List()
        {
            return _entries;
        }
    }
}
