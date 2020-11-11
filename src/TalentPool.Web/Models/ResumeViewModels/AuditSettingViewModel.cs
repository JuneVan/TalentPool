using System;
using System.Collections.Generic;

namespace TalentPool.Web.Models.ResumeViewModels
{
    public class AuditSettingViewModel
    {
        public List<AuditSettingModel> AuditSettings { get; set; }
        public List<AudtiSettingUserModel> Users { get; set; }
    }

    public class AudtiSettingUserModel
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; }
    }
    public class AuditSettingModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
