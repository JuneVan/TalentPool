namespace Une.TalentPool.Infrastructure.Notify
{
    public interface INotifier
    {
        void Add(NotifyType type, string message);
        NotifyEntry[] NotifyEntries { get; }
    }
}
