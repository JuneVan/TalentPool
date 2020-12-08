namespace TalentPool.AspNetCore.Mvc.Notify
{
    public interface INotifySerializer
    {
        string Serialize(NotifyEntry[] notifyEntries);
        NotifyEntry[] Deserialize(string value);
    }
}
