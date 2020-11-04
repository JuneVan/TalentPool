using System;
using TalentPool.Entities;

namespace TalentPool.Dictionaries
{
    public class DictionaryItem : Entity
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Guid DictionaryId { get; set; } 
    }
}
