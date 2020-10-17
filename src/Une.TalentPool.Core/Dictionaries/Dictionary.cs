using System.Collections.Generic;
using Une.TalentPool.Entities;

namespace Une.TalentPool.Dictionaries
{
    public class Dictionary : Entity
    { 
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ICollection<DictionaryItem> DictionaryItems { get; set; }
    }
}
