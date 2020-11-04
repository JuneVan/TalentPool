using System.ComponentModel.DataAnnotations;

namespace TalentPool.Web.Models.SettingViewModels
{
    public class EmailSettingViewModel
    {
        /// <summary>
        /// SMTP主机名 
        /// </summary>
        [Required(ErrorMessage = "请输入SMTP主机地址。")]
        public string HostName { get; set; }
        /// <summary>
        /// SMTP主机端口
        /// </summary>
        [Required(ErrorMessage = "请输入SMTP主机端口。")]
        public int Port { get; set; }
        /// <summary>
        /// 邮箱账户
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱账户。")]
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱密码。")]
        public string Password { get; set; }
        /// <summary>
        /// 邮箱发件人名称
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱发件人名称。")]
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否启用邮件发送
        /// </summary>
        public bool Enable { get; set; }
    }
}
