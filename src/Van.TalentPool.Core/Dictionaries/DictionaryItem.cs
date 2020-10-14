using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Dictionaries
{
    public class DictionaryItem : Entity
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Guid DictionaryId { get; set; }
        public Dictionary Dictionary { get; set; }
    }
}
