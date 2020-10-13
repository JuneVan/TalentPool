﻿using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Configurations
{
    public class SettingValue : Entity, ICreationAudited, IModificationAudited
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
