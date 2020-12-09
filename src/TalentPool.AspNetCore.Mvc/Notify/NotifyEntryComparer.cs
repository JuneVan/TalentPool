using System.Collections.Generic;

namespace TalentPool.AspNetCore.Mvc.Notify
{
    internal class NotifyEntryComparer : IEqualityComparer<NotifyEntry>
    { 

        public bool Equals(NotifyEntry x, NotifyEntry y)
        {
            return x.Type == y.Type && x.Message == y.Message;
        }

        public int GetHashCode(NotifyEntry obj)
        {
            return obj.GetHashCode() & 23 * obj.Type.GetHashCode();
        }
    }
}
