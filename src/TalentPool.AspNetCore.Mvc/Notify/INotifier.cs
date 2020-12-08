namespace TalentPool.AspNetCore.Mvc.Notify
{
    public interface INotifier
    {
        void Add(NotifyType type, string message);
        NotifyEntry[] NotifyEntries { get; }
    }
}
