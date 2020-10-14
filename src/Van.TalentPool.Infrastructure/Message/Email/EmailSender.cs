using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Threading.Tasks;
using Van.TalentPool.Configurations;

namespace Van.TalentPool.Infrastructure.Message.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly ConfigurationManager _settingManager;
        public EmailSender(ConfigurationManager settingManager,
             ILogger<EmailSender> logger)
        {
            _settingManager = settingManager;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailEntry entry)
        {
            var emailSetting = await _settingManager.GetSettingAsync<EmailSetting>();
            if (emailSetting.Enable)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailSetting.HostName, emailSetting.UserName));
                message.To.Add(new MailboxAddress(entry.ToName, entry.ToEmailAddress));//接收账号
                message.Subject = entry.Subject;
                message.Body = new TextPart("html") { Text = entry.Body };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(emailSetting.HostName, emailSetting.Port);
                    await client.AuthenticateAsync(emailSetting.UserName, emailSetting.Password);
                    await client.SendAsync(message);
                }
            }
            else
                _logger.LogWarning("Email setting is not enabled");
        }
    }
}
