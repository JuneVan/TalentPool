using System;
using Van.TalentPool.Entities;

namespace Van.TalentPool.Resumes
{
    public class ResumeAuditSetting : Entity
    {
        /// <summary>
        /// 用户
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }
    }
}
