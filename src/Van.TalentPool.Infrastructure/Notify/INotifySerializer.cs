﻿namespace Van.TalentPool.Infrastructure.Notify
{
    public interface INotifySerializer
    {
        string Serialize(NotifyEntry[] notifyEntries);
        NotifyEntry[] Deserialize(string value);
    }
}
