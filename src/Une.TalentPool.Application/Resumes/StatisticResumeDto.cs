﻿using System;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.Application.Resumes
{
    public class StatisticResumeDto
    {
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public string CreatorUserPhoto { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid OwnerUserId { get; set; } 
        public AuditStatus AuditStatus { get; set; }
        public bool Enable { get; set; }
        public bool ActiveDelivery { get; set; }
        public string JobName { get; set; }
    }
}
