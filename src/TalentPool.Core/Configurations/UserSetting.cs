using System;

namespace TalentPool.Configurations
{
    public class UserSetting : ISettingDefinition
    {
        /// <summary>
        /// 是否允许注册新用户
        /// </summary>
        public bool AllowedForNewUsers { get; set; }
       
        /// <summary>
        /// 必须已验证邮箱
        /// </summary>
        public bool RequireConfirmedEmail { get; set; }
        /// <summary>
        /// 必须验证手机号
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; }
        /// <summary>
        /// 必须确认账户
        /// </summary>
        public bool RequireConfirmedAccount { get; set; }

        /// <summary>
        /// 允许登录失败锁定
        /// </summary>
        public bool LockoutOnFailure { get; set; }
        /// <summary>
        /// 最大失败登录次数
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;
        /// <summary>
        /// 默认登录失败后锁定账户时间
        /// </summary>
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5);
    }
}
