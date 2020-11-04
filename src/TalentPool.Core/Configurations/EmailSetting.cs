namespace TalentPool.Configurations
{
    public class EmailSetting : ISettingDefinition
    {
        /// <summary>
        /// SMTP主机名 
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// SMTP主机端口
        /// </summary>
        public int Port { get; set; } = 25;
        /// <summary>
        /// 邮箱账户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱发件人名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否启用邮件发送
        /// </summary>
        public bool Enable { get; set; }
    }
}
