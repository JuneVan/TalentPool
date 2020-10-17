﻿using System;
using System.Collections.Generic;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.Application.Resumes
{
    public class ResumeDetailDto : Dto
    {
        public string Name { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public Guid OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public Guid? InvestigationId { get; set; }
        public AuditStatus AuditStatus { get; set; }
        public bool Enable { get; set; }
        public string PlatformName { get; set; }
        public string EnableReason { get; set; }
        public string PlatformId { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string LastModifierUserName { get; set; }
        public DateTime? LastModificationTime { get; set; } 
        public List<ResumeAuditRecordDto> ResumeAuditRecords { get; set; }
        public bool ActiveDelivery { get; set; }
    }
}
