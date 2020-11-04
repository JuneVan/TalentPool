using System;

namespace TalentPool.Configurations
{
    public class UserCustomSetting : IUserSettingDefinition
    {
        public Guid OwnerUserId { get; set; }
        /// <summary>
        /// 默认仅查看我的数据
        /// </summary>
        public bool DefaultOnlySeeMyselfData { get; set; } = true;
    }
}
