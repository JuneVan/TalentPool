using System.Collections.Generic;

namespace Van.TalentPool.Infrastructure.Notify
{
    public interface INotifier
    {
        void Add(NotifyType type, string message);
        IList<NotifyEntry> List();
    }
}
